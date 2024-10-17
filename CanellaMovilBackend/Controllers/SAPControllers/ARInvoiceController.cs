using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Service.SAPService;
using CanellaMovilBackend.Models;
using SAPbobsCOM;
using CanellaMovilBackend.Models.SAPModels.ARInvoice;
using static CanellaMovilBackend.Models.SAPModels.ARInvoice.RequestDataInvoice;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de la tabla OINV
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class ARInvoiceController : Controller
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public ARInvoiceController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Obtiene el top 100 facturas de un CardCode
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetInvoiceBusinessPartner(RequestGetInvoice request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                // Obtener el objeto BusinessPartners de la API de DI
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery("SELECT TOP(100) DocNum, DocEntry, DocDate, Cardcode, CardName, NumAtCard, DocStatus FROM OINV WITH (NOLOCK) WHERE CardCode='" + request.CardCode + "' order by DocDate desc");
                if (recordset.RecordCount > 0)
                {
                    List<object>? ListOINV = [];
                    while (!recordset.EoF)
                    {
                        OINV invoice = new()
                        {
                            DocDate = Convert.ToString(recordset.Fields.Item("DocDate").Value),
                            DocNum = Convert.ToString(recordset.Fields.Item("DocNum").Value),
                            DocEntry = Convert.ToString(recordset.Fields.Item("DocEntry").Value),
                            CardCode = Convert.ToString(recordset.Fields.Item("CardCode").Value),
                            CardName = Convert.ToString(recordset.Fields.Item("CardName").Value),
                            NumAtCard = Convert.ToString(recordset.Fields.Item("NumAtCard").Value),
                            DocStatus = Convert.ToString(recordset.Fields.Item("DocStatus").Value)
                        };

                        Recordset recordsetDetalle = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetDetalle.DoQuery("SELECT T1.DocEntry, T1.ItemCode, T1.Dscription, T1.Quantity,  T1.LineTotal, T1.WhsCode FROM INV1 T1 WITH (NOLOCK) WHERE T1.DocEntry = " + invoice.DocEntry);
                        while (!recordsetDetalle.EoF)
                        {
                            INV1 item = new()
                            {
                                DocEntry = Convert.ToString(recordsetDetalle.Fields.Item("DocEntry").Value),
                                ItemCode = Convert.ToString(recordsetDetalle.Fields.Item("ItemCode").Value),
                                Dscription = Convert.ToString(recordsetDetalle.Fields.Item("Dscription").Value),
                                Quantity = Convert.ToString(recordsetDetalle.Fields.Item("Quantity").Value),
                                LineTotal = Convert.ToString(recordsetDetalle.Fields.Item("LineTotal").Value),
                                Whscode = Convert.ToString(recordsetDetalle.Fields.Item("WhsCode").Value)
                            };
                            invoice?.Items?.Add(item);
                            recordsetDetalle.MoveNext();
                        }
                        ListOINV?.Add(new { invoice?.DocNum, invoice?.DocEntry, invoice?.CardCode, invoice?.CardName, invoice?.NumAtCard, invoice?.DocStatus, Items = invoice?.Items?.Select(x => new { x.DocEntry, x.ItemCode, x.Dscription, x.Quantity, x.LineTotal, x.Whscode }) });
                        recordset.MoveNext();
                    }
                    return Ok(ListOINV);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se encontro facturas relacionas con el CardCode: " + request.CardCode });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la cotización: DocNum = " + request.CardCode + " Error: " + ex.Message });
            }
        }
    }
}
