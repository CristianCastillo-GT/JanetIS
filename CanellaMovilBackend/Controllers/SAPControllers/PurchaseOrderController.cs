using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.Purchase_Order;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de Pagos Recibidos
    /// </summary>
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    //[ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class PurchaseOrderController : ControllerBase
    {

        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public PurchaseOrderController(ISAPService sapService)
        {
            this.sapService = sapService;
        }


        /// <summary>
        /// Cancela una Orden de Compra/Pedido
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CancelPurchaseOrder(int DocNum)
        {
            try
            {
                // Establecer la conexión con SAP
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;

                // Verificar que la conexión se haya establecido correctamente
                if (company == null || !company.Connected)
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "Error de conexión a SAP. La conexión no se pudo establecer.", Code = string.Empty });
                }

                // Obtener el objeto de orden de compra a partir del DocNum
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $"SELECT DocEntry, CANCELED FROM OPOR WHERE DocNum = '{DocNum}'";
                recordset.DoQuery(query);

                if (!recordset.EoF)
                {
                    // Verificar si el documento ya está cancelado
                    string isCanceled = recordset.Fields.Item("CANCELED").Value.ToString();
                    if (isCanceled == "Y")
                    {
                        return Conflict(new MessageAPI()
                        {
                            Result = "Fail",
                            Message = "La orden de compra ya se encuentra cancelada.",
                            Code = DocNum.ToString()
                        });
                    }

                    // Obtener el DocEntry correspondiente
                    int docEntry = int.Parse(recordset.Fields.Item("DocEntry").Value.ToString());

                    // Verificar si hay líneas cerradas en la orden de compra
                    Recordset lineCheck = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    string lineQuery = $"SELECT COUNT(*) AS LineasCerradas FROM POR1 WHERE DocEntry = {docEntry} AND LineStatus = 'C'";
                    lineCheck.DoQuery(lineQuery);

                    if (lineCheck.Fields.Item("LineasCerradas").Value.ToString() != "0")
                    {
                        return Conflict(new MessageAPI()
                        {
                            Result = "Fail",
                            Message = "No se puede cancelar la orden de compra porque contiene al menos una línea cerrada.",
                            Code = DocNum.ToString()
                        });
                    }

                    // Obtener el objeto de orden de compra basado en DocEntry
                    Documents purchaseOrder = (Documents)company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                    if (purchaseOrder.GetByKey(docEntry))
                    {
                        // Cancelar la orden de compra
                        if (purchaseOrder.Cancel() == 0)
                        {
                            // Verificar la cancelación en SAP
                            var canceledDocEntry = company.GetNewObjectKey();
                            Documents canceledOrder = (Documents)company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);

                            if (int.TryParse(canceledDocEntry, out int canceledDocEntryParsed) &&
                                canceledOrder.GetByKey(canceledDocEntryParsed))
                            {
                                return Ok(new MessageAPI()
                                {
                                    Result = "OK",
                                    Message = "Orden de compra cancelada exitosamente.",
                                    Code = canceledOrder.DocNum.ToString()
                                });
                            }
                            else
                            {
                                return Conflict(new MessageAPI()
                                {
                                    Result = "Fail",
                                    Message = "No se verificó la cancelación correctamente.",
                                    Code = string.Empty
                                });
                            }
                        }
                        else
                        {
                            // Manejo de errores de SAP
                            company.GetLastError(out int errCode, out string errMsg);
                            return Conflict(new MessageAPI()
                            {
                                Result = "Fail",
                                Message = $"Error al cancelar la orden de compra: {errMsg} (Código: {errCode})",
                                Code = DocNum.ToString()
                            });
                        }
                    }
                    else
                    {
                        return Conflict(new MessageAPI()
                        {
                            Result = "Fail",
                            Message = $"No se encontró la orden de compra con DocEntry: {docEntry}",
                            Code = DocNum.ToString()
                        });
                    }
                }
                else
                {
                    return Conflict(new MessageAPI()
                    {
                        Result = "Fail",
                        Message = $"El DocNum proporcionado ({DocNum}) no se encontró en la base de datos.",
                        Code = DocNum.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                return Conflict(new MessageAPI()
                {
                    Result = "Fail",
                    Message = "No se pudo cancelar la orden de compra - error: " + ex.Message,
                    Code = DocNum.ToString()
                });
            }
        }


        /// <summary>
        /// Cierra una Orden de Compra/Pedido
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult ClosePurchaseOrder(int DocNum)
        {
            try
            {
                // Establecer la conexión con SAP
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;

                // Verificar que la conexión se haya establecido correctamente
                if (company == null || !company.Connected)
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "Error de conexión a SAP. La conexión no se pudo establecer.", Code = string.Empty });
                }

                // Obtener el objeto de orden de compra a partir del DocNum
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $"SELECT DocEntry, DocStatus FROM OPOR WHERE DocNum = '{DocNum}'";
                recordset.DoQuery(query);

                if (!recordset.EoF)
                {
                    // Verificar si el documento ya está cerrado
                    string docStatus = recordset.Fields.Item("DocStatus").Value.ToString();
                    if (docStatus == "C")
                    {
                        return Conflict(new MessageAPI()
                        {
                            Result = "Fail",
                            Message = "La orden de compra ya se encuentra cerrada.",
                            Code = DocNum.ToString()
                        });
                    }

                    // Obtener el DocEntry correspondiente
                    int docEntry = int.Parse(recordset.Fields.Item("DocEntry").Value.ToString());

                    // Obtener el objeto de orden de compra basado en DocEntry
                    Documents purchaseOrder = (Documents)company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                    if (purchaseOrder.GetByKey(docEntry))
                    {
                        // Cerrar la orden de compra
                        if (purchaseOrder.Close() == 0)
                        {
                            // Verificar el cierre en SAP
                            var closedDocEntry = company.GetNewObjectKey();
                            Documents closedOrder = (Documents)company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);

                            if (int.TryParse(closedDocEntry, out int closedDocEntryParsed) &&
                                closedOrder.GetByKey(closedDocEntryParsed))
                            {
                                return Ok(new MessageAPI()
                                {
                                    Result = "OK",
                                    Message = "Orden de compra cerrada exitosamente.",
                                    Code = closedOrder.DocNum.ToString()
                                });
                            }
                            else
                            {
                                return Conflict(new MessageAPI()
                                {
                                    Result = "Fail",
                                    Message = "No se verificó el cierre correctamente.",
                                    Code = string.Empty
                                });
                            }
                        }
                        else
                        {
                            // Manejo de errores de SAP
                            company.GetLastError(out int errCode, out string errMsg);
                            return Conflict(new MessageAPI()
                            {
                                Result = "Fail",
                                Message = $"Error al cerrar la orden de compra: {errMsg} (Código: {errCode})",
                                Code = DocNum.ToString()
                            });
                        }
                    }
                    else
                    {
                        return Conflict(new MessageAPI()
                        {
                            Result = "Fail",
                            Message = $"No se encontró la orden de compra con DocEntry: {docEntry}",
                            Code = DocNum.ToString()
                        });
                    }
                }
                else
                {
                    return Conflict(new MessageAPI()
                    {
                        Result = "Fail",
                        Message = $"El DocNum proporcionado ({DocNum}) no se encontró en la base de datos.",
                        Code = DocNum.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                return Conflict(new MessageAPI()
                {
                    Result = "Fail",
                    Message = "No se pudo cerrar la orden de compra - error: " + ex.Message,
                    Code = DocNum.ToString()
                });
            }
        }



    }


}
