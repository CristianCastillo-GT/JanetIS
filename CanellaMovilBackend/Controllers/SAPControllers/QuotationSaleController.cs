using CanellaMovilBackend.Filters;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Service.SAPService;
using CanellaMovilBackend.Models;
using SAPbobsCOM;
using CanellaMovilBackend.Models.SAPModels.SalesQuotation;
using static CanellaMovilBackend.Models.SAPModels.SalesQuotation.RequestDataQuotation;
using CanellaMovilBackend.Filters.UserFilter;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador del socio de negocios
    /// </summary>
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class QuotationSaleController : Controller
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public QuotationSaleController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Crea una Oferta de Ventas en SAP
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreateQuotation(OQUT OQUT)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                Documents saleQuotation = (Documents)company.GetBusinessObject(BoObjectTypes.oQuotations);
                //Encabezado
                saleQuotation.DocDate = Convert.ToDateTime(OQUT.DocDate);
                saleQuotation.DocDueDate = Convert.ToDateTime(OQUT.DocDueDate);
                saleQuotation.TaxDate = Convert.ToDateTime(OQUT.TaxDate);
                saleQuotation.CardCode = OQUT.CardCode;
                saleQuotation.NumAtCard = OQUT.NumAtCard;
                saleQuotation.DocCurrency = OQUT.DocCur;
                saleQuotation.Comments = OQUT.Comments;
                saleQuotation.Series = Convert.ToInt32(OQUT.Series);
                saleQuotation.SalesPersonCode = Convert.ToInt32(OQUT.SlpCode);
                saleQuotation.GroupNumber = Convert.ToInt32(OQUT.GroupNumber);

                //Logistica
                saleQuotation.ShipToCode = Convert.ToString(OQUT.ShipToCode);
                saleQuotation.PayToCode = Convert.ToString(OQUT.PaytoCode);

                //Campos de usuario 
                saleQuotation.UserFields.Fields.Item("U_DoctoFiscal").Value = OQUT.U_DoctoFiscal;
                saleQuotation.UserFields.Fields.Item("U_NoOrdenCompra").Value = OQUT.U_NoOrdenCompra;
                saleQuotation.UserFields.Fields.Item("U_DoctoRef").Value = OQUT.U_DoctoRef;
                saleQuotation.UserFields.Fields.Item("U_DoctoRefNo").Value = OQUT.U_DoctoRefNo;
                saleQuotation.UserFields.Fields.Item("U_DoctoGenServ").Value = OQUT.U_DoctoGenServ;
                saleQuotation.UserFields.Fields.Item("U_MotivoDesc").Value = OQUT.U_MotivoDesc;
                saleQuotation.UserFields.Fields.Item("U_SNNit").Value = OQUT.U_SNNit;
                saleQuotation.UserFields.Fields.Item("U_SNNombre").Value = OQUT.U_SNNombre;
                saleQuotation.UserFields.Fields.Item("U_PersonaContacto").Value = OQUT.U_PersonaContacto;
                saleQuotation.UserFields.Fields.Item("U_PersonaTel").Value = OQUT.U_PersonaTel;
                saleQuotation.UserFields.Fields.Item("U_PersonaCorreo").Value = OQUT.U_PersonaCorreo;

                //Detalle
                foreach (var item in OQUT?.Items ?? [])
                {
                    saleQuotation.Lines.ItemCode = item.ItemCode;
                    saleQuotation.Lines.Quantity = Convert.ToDouble(item.Quantity);
                    //saleQuotation.Lines.PriceAfterVAT = Convert.ToDouble(item.PriceAfVAT);
                    saleQuotation.Lines.DiscountPercent = Convert.ToDouble(item.DiscPrcnt);
                    saleQuotation.Lines.UnitPrice = Convert.ToDouble(item.PriceItem);
                    saleQuotation.Lines.Currency = item.Currency;
                    saleQuotation.Lines.WarehouseCode = item.Whscode;
                    saleQuotation.Lines.SalesPersonCode = Convert.ToInt32(item.SlpCode);
                    saleQuotation.Lines.CostingCode = item.OcrCode;
                    saleQuotation.Lines.TaxCode = item.TaxCode;
                    saleQuotation.Lines.FreeText = item.FreeTxt;
                    saleQuotation.Lines.COGSCostingCode = item.COGSCostingCode;

                    saleQuotation.Lines.UserFields.Fields.Item("U_Solicitado").Value = item.U_Solicitado;
                    saleQuotation.Lines.UserFields.Fields.Item("U_Tipo").Value = item.U_Tipo;
                    saleQuotation.Lines.Add();

                }
                // Agregar el socio de negocios a la base de datos
                int result = saleQuotation.Add();
                if (result != 0)
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear la oferta de ventas: " + company.GetLastErrorDescription() });
                }
                company.GetNewObjectCode(out string DocEntry);
                saleQuotation.GetByKey(Convert.ToInt32(DocEntry));
                return Ok(new { Result = "OK", Message = "El documento fue creado correctamente.", Code = saleQuotation.DocNum.ToString(), DocEntry = saleQuotation.DocEntry.ToString()});
            }
            catch (Exception Ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear la oferta de ventas: " + Ex });
            }
        }

        /// <summary>
        /// Obtiene una Oferta de Ventas en SAP por el DocNum
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Consulta exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetQuotation(RequestGetQuotation request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                // Obtener el objeto BusinessPartners de la API de DI
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery("SELECT DocNum, DocEntry, DocDate, Cardcode, CardName, NumAtCard, DocStatus FROM OQUT WITH (NOLOCK) WHERE DocNum=" + request.DocNum);
                
                OQUT quotation = new();
                if (recordset.RecordCount > 0)
                {
                    quotation = new OQUT
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
                    recordsetDetalle.DoQuery("SELECT T1.DocEntry, T1.ItemCode, T1.Dscription, T1.Quantity,  T1.LineTotal, T1.WhsCode FROM QUT1 T1 WITH (NOLOCK) WHERE T1.DocEntry = " + quotation.DocEntry);
                    while (!recordsetDetalle.EoF)
                    {
                        QUT1 item = new()
                        {
                            DocEntry = Convert.ToString(recordsetDetalle.Fields.Item("DocEntry").Value),
                            ItemCode = Convert.ToString(recordsetDetalle.Fields.Item("ItemCode").Value),
                            Dscription = Convert.ToString(recordsetDetalle.Fields.Item("Dscription").Value),
                            Quantity = Convert.ToString(recordsetDetalle.Fields.Item("Quantity").Value),
                            LineTotal = Convert.ToString(recordsetDetalle.Fields.Item("LineTotal").Value),
                            Whscode = Convert.ToString(recordsetDetalle.Fields.Item("WhsCode").Value)
                        };
                        quotation?.Items?.Add(item);
                        recordsetDetalle.MoveNext();
                    }
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la cotización: DocNum=" + request.DocNum });
                }
                return Ok(new { quotation?.DocNum, quotation?.DocEntry, quotation?.CardCode, quotation?.CardName, quotation?.NumAtCard, quotation?.DocStatus, Items = quotation?.Items?.Select(x => new { x.DocEntry, x.ItemCode, x.Dscription, x.Quantity, x.LineTotal, x.Whscode }) });
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la cotización: DocNum = " + request.DocNum + " Error: " + ex.Message });
            }
        }
    }
}
