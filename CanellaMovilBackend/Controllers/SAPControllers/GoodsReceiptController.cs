using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.Goods_Receipt;
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
    public class GoodsReceiptController: ControllerBase
    {

        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public GoodsReceiptController(ISAPService sapService)
        {
            this.sapService = sapService;
        }


        /// <summary>
        /// Cancela ítems específicos de una entrada de mercancía en SAP usando el DocNum
        /// </summary>
        /// <returns>Códigos de respuesta</returns>
        /// <response code="200">Cancelación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CancelGoodsReceipt(int DocNum)
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

                // Obtener el objeto de entrada de mercancía a partir del DocNum
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = $"SELECT DocEntry, CANCELED FROM OPDN WHERE DocNum = '{DocNum}'";
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
                            Message = "La entrada de mercancía ya se encuentra cancelada.",
                            Code = DocNum.ToString()
                        });
                    }

                    // Obtener el DocEntry correspondiente
                    int docEntry = int.Parse(recordset.Fields.Item("DocEntry").Value.ToString());

                    // Obtener el objeto de entrada de mercancía basado en DocEntry
                    Documents goodsReceipt = (Documents)company.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);
                    if (goodsReceipt.GetByKey(docEntry))
                    {
                        // Actualizar el documento en SAP
                        if (goodsReceipt.Cancel() == 0)
                        {
                            // Verificar la cancelación en SAP
                            var canceledDocEntry = company.GetNewObjectKey();
                            Documents canceledOrder = (Documents)company.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

                            if (int.TryParse(canceledDocEntry, out int canceledDocEntryParsed) &&
                                canceledOrder.GetByKey(canceledDocEntryParsed))
                            {
                                return Ok(new MessageAPI()
                                {
                                    Result = "OK",
                                    Message = "Entrada de mercancía cancelada exitosamente.",
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
                                Message = $"Error al cancelar la entrada de mercancía: {errMsg} (Código: {errCode})",
                                Code = DocNum.ToString()
                            });
                        }
                    }
                    else
                    {
                        return Conflict(new MessageAPI()
                        {
                            Result = "Fail",
                            Message = $"No se encontró la entrada de mercancía con DocEntry: {docEntry}",
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
                    Message = "No se pudo cancelar la entrada de mercancía - error: " + ex.Message,
                    Code = DocNum.ToString()
                });
            }
        }




    }
}
