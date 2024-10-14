using CanellaMovilBackend.Filters.UserFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Service.SAPService;
using CanellaMovilBackend.Models;
using SAPbobsCOM;
using CanellaMovilBackend.Models.SAPModels.SalesOrder;
using static CanellaMovilBackend.Models.SAPModels.SalesOrder.RequestDataOrder;
using ConexionesSQL.SAP;
using ConexionesSQL.Models;
using CanellaMovilBackend.Utils;
using System.Data;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador del socio de negocios
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class OrderSaleController : Controller
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public OrderSaleController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Crea una Orden de Venta en SAP
        /// </summary>
        /// <param name="ORDR">Objeto de envio</param>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreateOrder(SaleOrder ORDR)
        {
            Company company = sapService.SAPB1();
            try
            {
                Documents saleOrder = (Documents)company.GetBusinessObject(BoObjectTypes.oOrders);
                saleOrder.CardCode = ORDR.CardCode;
                saleOrder.DocDate = ORDR.DocDate;
                saleOrder.DocDueDate = ORDR.DocDueDate;
                saleOrder.TaxDate = ORDR.TaxDate;
                saleOrder.Series = ORDR.Series;
                saleOrder.SalesPersonCode = ORDR.SalesPersonCode;
                saleOrder.DocumentsOwner = ORDR.DocumentsOwner;
                saleOrder.Comments = ORDR.Comments;

                // User Fields
                saleOrder.UserFields.Fields.Item("U_Autorizador").Value = ORDR.U_Autorizador;
                saleOrder.UserFields.Fields.Item("U_LlamadaServicio").Value = ORDR.U_LlamadaServicio;
                saleOrder.UserFields.Fields.Item("U_SNNit").Value = ORDR.U_SNNit;
                saleOrder.UserFields.Fields.Item("U_SNNombre").Value = ORDR.U_SNNombre;
                saleOrder.UserFields.Fields.Item("U_DoctoGenServ").Value = ORDR.U_DoctoGenServ;
                saleOrder.UserFields.Fields.Item("U_Solicitante").Value = ORDR.U_Solicitante;
                if (!string.IsNullOrEmpty(ORDR.U_Contrato) && !string.IsNullOrWhiteSpace(ORDR.U_Contrato))
                {
                    saleOrder.UserFields.Fields.Item("U_Contrato").Value = ORDR.U_Contrato;
                }
                if (!string.IsNullOrEmpty(ORDR.U_ContratoMant) && !string.IsNullOrWhiteSpace(ORDR.U_ContratoMant))
                {
                    saleOrder.UserFields.Fields.Item("U_ContratoMant").Value = ORDR.U_ContratoMant;
                }
                saleOrder.UserFields.Fields.Item("U_Requisicion").Value = ORDR.U_Requisicion;

                // Lines
                foreach (var item in ORDR?.Items ?? [])
                {
                    saleOrder.Lines.ItemCode = item.ItemCode;
                    saleOrder.Lines.Quantity = item.Quantity;
                    saleOrder.Lines.UnitPrice = item.UnitPrice;
                    saleOrder.Lines.DiscountPercent = item.DiscountPercent;
                    saleOrder.Lines.WarehouseCode = item.WarehouseCode;
                    saleOrder.Lines.TaxCode = item.TaxCode;
                    saleOrder.Lines.CostingCode = item.CostingCode;
                    saleOrder.Lines.COGSCostingCode = item.COGSCostingCode;
                    saleOrder.Lines.UserFields.Fields.Item("U_Tipo").Value = item.U_Tipo;
                    saleOrder.Lines.Add();

                }
                if (saleOrder.Add() == 0)
                {
                    var docEntry = company.GetNewObjectKey();
                    Documents saleOrderCreated = (Documents)company.GetBusinessObject(BoObjectTypes.oOrders);
                    _ = int.TryParse(docEntry, out int docEntry2);
                    if (saleOrderCreated.GetByKey(docEntry2))
                    {
                        sapService.SAPB1_DISCONNECT(company);
                        return Ok(new MessageAPI() {
                            Result = "OK",
                            Message = "Creada la orden de ventas exitosamente",
                            Code = docEntry,
                            CodeNum = saleOrderCreated.DocNum.ToString()
                        });
                    }
                    else
                    {
                        sapService.SAPB1_DISCONNECT(company);
                        return Ok(new MessageAPI() {
                            Result = "OK",
                            Message = "Se creo el documento pero no se pudo obtener su DocNum, se enviara el docentry",
                            Code = docEntry,
                            CodeNum = "0"
                        });
                    }
                }
                else
                {
                    company.GetLastError(out int errCode, out string errMsg);
                    sapService.SAPB1_DISCONNECT(company);
                    return Conflict(new MessageAPI() { Result = "Fail", Message = $"No se creo la orden de venta: {errMsg}", Code = string.Empty });
                }
            }
            catch(Exception ex){
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear la orden de venta: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene las ordenes de venta por vendedor
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetSalesOrders(RequestGetOrders request)
        {
            Company company = sapService.SAPB1();
            try
            {
                if (request.InitialDate == null || request.FinalDate == null || request.InitialDate == "" || request.FinalDate == "")
                {
                    request.InitialDate = DateTime.Now.ToString("yyyy-MM-dd");
                    request.FinalDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                // Obtener el objeto BusinessPartners de la API de DI
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery("SELECT TOP(175) DocNum, DocEntry, DocDate, Cardcode, CardName, NumAtCard, DocStatus, CANCELED FROM ORDR WITH (NOLOCK) WHERE (DocDate  between '" + request.InitialDate + "' and ' " + request.FinalDate + "') and SlpCode='" + request.SlpCode + "' "+ (request.status == "O" || request.status == "C" ? "and DocStatus='"+request.status+"'" : "") +" order by DocDate desc");

                if (recordset.RecordCount > 0)
                {
                    List<object>? ListORDR = [];
                    while (!recordset.EoF)
                    {
                        ORDR order = new()
                        {
                            DocDate = Convert.ToString(recordset.Fields.Item("DocDate").Value),
                            DocNum = Convert.ToString(recordset.Fields.Item("DocNum").Value),
                            DocEntry = Convert.ToString(recordset.Fields.Item("DocEntry").Value),
                            CardCode = Convert.ToString(recordset.Fields.Item("CardCode").Value),
                            CardName = Convert.ToString(recordset.Fields.Item("CardName").Value),
                            NumAtCard = Convert.ToString(recordset.Fields.Item("NumAtCard").Value),
                            DocStatus = Convert.ToString(recordset.Fields.Item("DocStatus").Value),
                            CANCELED = Convert.ToString(recordset.Fields.Item("CANCELED").Value)
                        };

                        Recordset recordsetDetalle = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetDetalle.DoQuery("SELECT T1.DocEntry, T1.ItemCode, T1.Dscription, T1.Quantity,  T1.LineTotal, T1.WhsCode FROM RDR1 T1 WITH (NOLOCK) WHERE T1.DocEntry = " + order.DocEntry);
                        while (!recordsetDetalle.EoF)
                        {
                            RDR1 item = new()
                            {
                                DocEntry = Convert.ToString(recordsetDetalle.Fields.Item("DocEntry").Value),
                                ItemCode = Convert.ToString(recordsetDetalle.Fields.Item("ItemCode").Value),
                                Dscription = Convert.ToString(recordsetDetalle.Fields.Item("Dscription").Value),
                                Quantity = Convert.ToString(recordsetDetalle.Fields.Item("Quantity").Value),
                                LineTotal = Convert.ToString(recordsetDetalle.Fields.Item("LineTotal").Value),
                                Whscode = Convert.ToString(recordsetDetalle.Fields.Item("WhsCode").Value)
                            };
                            order?.Items?.Add(item);
                            recordsetDetalle.MoveNext();
                        }
                        ListORDR?.Add(new { order?.DocNum, order?.DocDate, order?.DocEntry, order?.CardCode, order?.CardName, order?.NumAtCard, order?.DocStatus, order?.CANCELED, Items = order?.Items?.Select(x => new { x.DocEntry, x.ItemCode, x.Dscription, x.Quantity, x.LineTotal, x.Whscode }) });
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(ListORDR);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(new MessageAPI() { Result = "OK", Message = "No se encontraron documentos relacionados con el codigo de vendedor: " + request.SlpCode });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo realizar la busqueda " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la orden de venta por DocEntry
        /// </summary>
        /// <param name="DocEntry">DocEntry del documento de SAP de la orden de venta</param>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="404">No se encontro la orden de venta</response>
        /// <response code="409">Mensaje de error</response>
        [HttpGet]
        [ProducesResponseType(typeof(SaleOrderOne), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetSaleOrder(int DocEntry)
        {
            try
            {
                var parameters = new List<Parametros>()
                {
                    new("DocEntry", DocEntry)
                };

                SP_ORDR query = new();
                var resultado = query.ORDR_GETSaleOrder(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    List<SaleOrderOne> ordr = ObjectConvert.CreateListFromDataTable<SaleOrderOne>(resultado.Datos);
                    if (ordr.Count > 0)
                    {
                        return Ok(ordr.First());
                    }
                    else
                    {
                        return NotFound(new MessageAPI() { Result = "Fail", Message = "No se encontro la orden de venta" });
                    }
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = resultado.MensajeDescripcion });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo realizar la busqueda: " + ex.Message });
            }
        }
    }
}
