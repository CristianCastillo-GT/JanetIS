using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.OutgoingPayments;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de Pagos Efectuados
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class OutgoingPaymentsController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public OutgoingPaymentsController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Crea un pago efectuado en SAP
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreatePayment(OVPM OVPM)
        {
            try
            {
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;

                Payments oVendorPayments = (Payments)company.GetBusinessObject(BoObjectTypes.oVendorPayments);
                oVendorPayments.DocType = (OVPM.DocType == "A" ? BoRcptTypes.rAccount : (OVPM.DocType == "C" ? BoRcptTypes.rCustomer : (OVPM.DocType == "S" ? BoRcptTypes.rSupplier : throw new InvalidOperationException("El tipo de pago no es válido"))));
                oVendorPayments.DocDate = DateTime.Parse(OVPM.DocDate);
                oVendorPayments.DueDate = DateTime.Parse(OVPM.DocDueDate);
                oVendorPayments.TaxDate = DateTime.Parse(OVPM.TaxDate);
                if (!string.IsNullOrWhiteSpace(OVPM.CardCode) && !string.IsNullOrEmpty(OVPM.CardCode))
                {
                    oVendorPayments.CardCode = OVPM.CardCode;
                }
                else
                {
                    oVendorPayments.CardName = OVPM.CardName;
                }
                oVendorPayments.DocCurrency = OVPM.DocCurr;
                oVendorPayments.DocRate = double.Parse(OVPM.DocRate);
                oVendorPayments.Remarks = OVPM.Comments;
                oVendorPayments.ControlAccount = OVPM.BpAct;

                
                oVendorPayments.CashAccount = OVPM.CashAcct;
                oVendorPayments.CheckAccount = OVPM.CheckAccount;
                oVendorPayments.TransferAccount = OVPM.TransferAccount;
                oVendorPayments.CashSum = 0;
                oVendorPayments.TransferSum = 0;


                oVendorPayments.UserFields.Fields.Item("U_OCDocNum").Value = OVPM.U_OCDocNum;
                oVendorPayments.UserFields.Fields.Item("U_TipoPagos").Value = OVPM.U_TipoPagos;
                oVendorPayments.UserFields.Fields.Item("U_PagoVerificado").Value = "S";     //Se debe de consultar este parametros
                oVendorPayments.UserFields.Fields.Item("U_TipoPago").Value = OVPM.U_TipoPago;
                oVendorPayments.UserFields.Fields.Item("U_AplicaRetencion").Value = "SI";   //Se debe de consultar este parametros


                //Verifica si el tipo es transferencia es cheque o transferencia
                foreach (PaymentMethod pay in OVPM.PaymentMethod ?? [])
                {
                    if (pay.Type == "CH")
                    {
                        oVendorPayments.Checks.CheckSum = double.Parse(pay.Sum);
                        oVendorPayments.Checks.CheckNumber = Convert.ToInt32(pay.ReferenceNumber);
                        oVendorPayments.Checks.CountryCode = pay.CountryCode;
                        oVendorPayments.Checks.BankCode = pay.BankCode;
                        oVendorPayments.Checks.Add();
                    }

                    if (pay.Type == "TR")
                    {
                        oVendorPayments.TransferSum += double.Parse(pay.Sum);
                        oVendorPayments.TransferDate = DateTime.Parse(pay.Date);
                        oVendorPayments.TransferReference = pay.ReferenceNumber;
                    }
                }

                //Se inserta el dato
                oVendorPayments.Add();


                if (company.GetLastErrorDescription() == "")
                    return Ok(new MessageAPI() { Result = "OK", Message = "Creado Correctamente." });
                else
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el registro - error: " + company.GetLastErrorDescription() });

            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el deposito - error: " + ex.Message });
            }
        }

        /// <summary>
        /// Crea un pago efectuado en SAP
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<MessageAPI>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult BulkCreatePayment(List<OVPM> OVPMList)
        {
            try
            {
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;

                List<MessageAPI> messageApi = [];
                foreach (OVPM OVPM in OVPMList)
                {
                    Payments? oVendorPayments = (Payments)company.GetBusinessObject(BoObjectTypes.oVendorPayments);

                    oVendorPayments.DocType = (OVPM.DocType == "A" ? BoRcptTypes.rAccount : (OVPM.DocType == "C" ? BoRcptTypes.rCustomer : (OVPM.DocType == "S" ? BoRcptTypes.rSupplier : throw new InvalidOperationException("El tipo de pago no es válido"))));
                    oVendorPayments.DocDate = DateTime.Parse(OVPM.DocDate);
                    oVendorPayments.DueDate = DateTime.Parse(OVPM.DocDueDate);
                    oVendorPayments.TaxDate = DateTime.Parse(OVPM.TaxDate);
                    if (!string.IsNullOrWhiteSpace(OVPM.CardCode) && !string.IsNullOrEmpty(OVPM.CardCode))
                    {
                        oVendorPayments.CardCode = OVPM.CardCode;
                    }
                    else
                    {
                        oVendorPayments.CardName = OVPM.CardName;
                    }
                    oVendorPayments.DocCurrency = OVPM.DocCurr;
                    oVendorPayments.DocRate = double.Parse(OVPM.DocRate);
                    oVendorPayments.Remarks = OVPM.Comments;
                    oVendorPayments.ControlAccount = OVPM.BpAct;
                    oVendorPayments.CashAccount = OVPM.CashAcct;
                    oVendorPayments.CheckAccount = OVPM.CheckAccount;
                    oVendorPayments.TransferAccount = OVPM.TransferAccount;
                    oVendorPayments.CashSum = 0;
                    oVendorPayments.CounterReference = OVPM.CounterRef;
                    oVendorPayments.Reference1 = OVPM.Ref1;
                    oVendorPayments.Reference2 = OVPM.Ref2;
                    oVendorPayments.JournalRemarks = OVPM.JrnlMemo;
                    oVendorPayments.TransferSum = 0;

                    oVendorPayments.UserFields.Fields.Item("U_OCDocNum").Value = OVPM.U_OCDocNum;
                    oVendorPayments.UserFields.Fields.Item("U_TipoPagos").Value = OVPM.U_TipoPagos;
                    oVendorPayments.UserFields.Fields.Item("U_PagoVerificado").Value = "S";
                    oVendorPayments.UserFields.Fields.Item("U_TipoPago").Value = OVPM.U_TipoPago;
                    oVendorPayments.UserFields.Fields.Item("U_AplicaRetencion").Value = "SI";

                    foreach (VPM4 account in OVPM.VPM4 ?? [])
                    {
                        oVendorPayments.AccountPayments.AccountCode = account.AccountCode;
                        oVendorPayments.AccountPayments.AccountName = account.AccountName;
                        oVendorPayments.AccountPayments.Decription = account.Decription;
                        oVendorPayments.AccountPayments.SumPaid = account.SumPaid;
                        oVendorPayments.AccountPayments.Add();
                    }

                    foreach (PaymentMethod pay in OVPM.PaymentMethod ?? [])
                    {
                        if (pay.Type == "CH")
                        {
                            oVendorPayments.Checks.CheckSum = double.Parse(pay.Sum);
                            oVendorPayments.Checks.CheckNumber = Convert.ToInt32(pay.ReferenceNumber);
                            oVendorPayments.Checks.CountryCode = pay.CountryCode;
                            oVendorPayments.Checks.BankCode = pay.BankCode;
                            oVendorPayments.Checks.Add();
                        }

                        if (pay.Type == "TR")
                        {
                            oVendorPayments.TransferSum += double.Parse(pay.Sum);
                            oVendorPayments.TransferDate = DateTime.Parse(pay.Date);
                            oVendorPayments.TransferReference = pay.ReferenceNumber;
                        }
                    }

                    if (oVendorPayments.Add()==0)
                    {
                        var docentry = company.GetNewObjectKey();
                        Payments PagoCreado = (Payments)company.GetBusinessObject(BoObjectTypes.oVendorPayments);
                        _ = int.TryParse(docentry, out int docentry2);
                        if ( PagoCreado.GetByKey(docentry2))
                        {
                            messageApi.Add(new MessageAPI() { Result = "OK", Message = "Se creo El pago a Cuenta", Code = PagoCreado.DocNum.ToString(), CodeNum = PagoCreado.CounterReference.ToString() });
                        }
                        else
                        {
                            messageApi.Add(new MessageAPI() { Result = "Fail", Message = "No se Termino el proceo Correctamente ", Code = string.Empty });
                        }
                    }
                    else
                    {
                        company.GetLastError(out int errCode, out string errMsg);
                        messageApi.Add(new MessageAPI() { Result = "Fail", Message = errMsg, Code = string.Empty });
                    }
                }
                return Ok(messageApi);                
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el deposito - error: " + ex.Message });
            }
        }
    }
}
