using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.IncomingPayments;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

namespace CanellaMovilBackend.Controllers.SAPControllers
{

    /// <summary>
    /// Controlador de Pagos Recibidos
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class IncomingPaymentsController : ControllerBase

    {

        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public IncomingPaymentsController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

       
        /// <summary>
        /// Crea pagos recibidos en SAP
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<MessageAPI>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult BulkCreatePayment(List<ORCT> ORCTList)
        {
            try
            {
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;

                List<MessageAPI> messageApi = [];
                foreach (ORCT ORCT in ORCTList)
                {
                    Payments? oIncomingPayments = (Payments)company.GetBusinessObject(BoObjectTypes.oIncomingPayments);

                    oIncomingPayments.DocType = (ORCT.DocType == "A" ? BoRcptTypes.rAccount : (ORCT.DocType == "C" ? BoRcptTypes.rCustomer : (ORCT.DocType == "S" ? BoRcptTypes.rSupplier : throw new InvalidOperationException("El tipo de pago no es válido"))));
                    oIncomingPayments.DocDate = DateTime.Parse(ORCT.DocDate);
                    oIncomingPayments.DueDate = DateTime.Parse(ORCT.DocDueDate);
                    oIncomingPayments.TaxDate = DateTime.Parse(ORCT.TaxDate);
                    if (!string.IsNullOrWhiteSpace(ORCT.CardCode) && !string.IsNullOrEmpty(ORCT.CardCode))
                    {
                        oIncomingPayments.CardCode = ORCT.CardCode;
                    }
                    else
                    {
                        oIncomingPayments.CardName = ORCT.CardName;
                    }
                    oIncomingPayments.DocCurrency = ORCT.DocCurr;
                    oIncomingPayments.DocRate = double.Parse(ORCT.DocRate);
                    oIncomingPayments.Remarks = ORCT.Comments;
                    oIncomingPayments.ControlAccount = ORCT.BpAct;
                    oIncomingPayments.CashAccount = ORCT.CashAcct;
                    oIncomingPayments.CheckAccount = ORCT.CheckAccount;
                    oIncomingPayments.TransferAccount = ORCT.TransferAccount;
                    oIncomingPayments.CashSum = 0;
                    oIncomingPayments.CounterReference = ORCT.CounterRef;
                    oIncomingPayments.Reference1 = ORCT.Ref1;
                    oIncomingPayments.JournalRemarks = ORCT.JrnlMemo;
                    oIncomingPayments.TransferSum = 0;
                    oIncomingPayments.Series = 244;

                    oIncomingPayments.UserFields.Fields.Item("U_OCDocNum").Value = ORCT.U_OCDocNum;
                    oIncomingPayments.UserFields.Fields.Item("U_TipoPagos").Value = ORCT.U_TipoPagos;
                    oIncomingPayments.UserFields.Fields.Item("U_PagoVerificado").Value = "N";
                    oIncomingPayments.UserFields.Fields.Item("U_TipoPago").Value = ORCT.U_TipoPago;
                    oIncomingPayments.UserFields.Fields.Item("U_AplicaRetencion").Value = "NO";


                    oIncomingPayments.UserFields.Fields.Item("U_DpsBoleta").Value = ORCT.U_DpsBoleta;
                    oIncomingPayments.UserFields.Fields.Item("U_BcoDpsBoleta").Value = ORCT.U_BcoDpsBoleta;

                    foreach (RCT4 account in ORCT.RCT4 ?? [])
                    {
                        oIncomingPayments.AccountPayments.AccountCode = account.AccountCode;
                        oIncomingPayments.AccountPayments.AccountName = account.AccountName;
                        oIncomingPayments.AccountPayments.Decription = account.Decription;
                        oIncomingPayments.AccountPayments.SumPaid = account.SumPaid;
                        oIncomingPayments.AccountPayments.Add();
                    }

                    foreach (IncomingPaymentMethod pay in ORCT.IncomingPaymentMethod ?? [])
                    {
                        if (pay.Type == "CH")
                        {
                            oIncomingPayments.Checks.CheckSum = double.Parse(pay.Sum);
                            oIncomingPayments.Checks.CheckNumber = Convert.ToInt32(pay.ReferenceNumber);
                            oIncomingPayments.Checks.CountryCode = pay.CountryCode;
                            oIncomingPayments.Checks.BankCode = pay.BankCode;
                            oIncomingPayments.Checks.Add();
                        }

                        if (pay.Type == "TR")
                        {
                            oIncomingPayments.TransferSum += double.Parse(pay.Sum);
                            oIncomingPayments.TransferDate = DateTime.Parse(pay.Date);
                            oIncomingPayments.TransferReference = pay.ReferenceNumber;

                        }

                        if (pay.Type == "EF")  // Si es un pago en efectivo
                        {
                            oIncomingPayments.CashSum += double.Parse(pay.Sum);  // Agregar el monto al campo CashSum
                        }
                    }

                    if (oIncomingPayments.Add() == 0)
                    {
                        var docentry = company.GetNewObjectKey();
                        Payments PagoCreado = (Payments)company.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                        _ = int.TryParse(docentry, out int docentry2);
                        if (PagoCreado.GetByKey(docentry2))
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

        /// <summary>
        /// Crea un pago Individual efectuado en SAP
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreateSinglePayment(ORCT ORCT)
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


                // Crear el objeto de pago
                Payments oIncomingPayments = (Payments)company.GetBusinessObject(BoObjectTypes.oIncomingPayments);

                // Asignar valores al objeto de pago
                oIncomingPayments.DocType = (ORCT.DocType == "A" ? BoRcptTypes.rAccount :
                                            (ORCT.DocType == "C" ? BoRcptTypes.rCustomer :
                                            (ORCT.DocType == "S" ? BoRcptTypes.rSupplier :
                                            throw new InvalidOperationException("El tipo de pago no es válido"))));
                oIncomingPayments.DocDate = DateTime.Parse(ORCT.DocDate);
                oIncomingPayments.DueDate = DateTime.Parse(ORCT.DocDueDate);
                oIncomingPayments.TaxDate = DateTime.Parse(ORCT.TaxDate);

                // Asignación de datos de cliente o proveedor
                if (!string.IsNullOrWhiteSpace(ORCT.CardCode) && !string.IsNullOrEmpty(ORCT.CardCode))
                {
                    oIncomingPayments.CardCode = ORCT.CardCode;
                }
                else
                {
                    oIncomingPayments.CardName = ORCT.CardName;
                }

                oIncomingPayments.DocCurrency = ORCT.DocCurr;
                oIncomingPayments.DocRate = double.Parse(ORCT.DocRate);
                oIncomingPayments.Remarks = ORCT.Comments;
                oIncomingPayments.ControlAccount = ORCT.BpAct;
                oIncomingPayments.CashAccount = ORCT.CashAcct;
                oIncomingPayments.CheckAccount = ORCT.CheckAccount;
                oIncomingPayments.TransferAccount = ORCT.TransferAccount;
                oIncomingPayments.CashSum = 0;
                oIncomingPayments.CounterReference = ORCT.CounterRef;
                oIncomingPayments.Reference1 = ORCT.Ref1;
                oIncomingPayments.JournalRemarks = ORCT.JrnlMemo;
                oIncomingPayments.TransferSum = 0;
                oIncomingPayments.Series = 244;

                // Campos personalizados
                oIncomingPayments.UserFields.Fields.Item("U_OCDocNum").Value = ORCT.U_OCDocNum;
                oIncomingPayments.UserFields.Fields.Item("U_TipoPagos").Value = ORCT.U_TipoPagos;
                oIncomingPayments.UserFields.Fields.Item("U_PagoVerificado").Value = "N";
                oIncomingPayments.UserFields.Fields.Item("U_TipoPago").Value = ORCT.U_TipoPago;
                oIncomingPayments.UserFields.Fields.Item("U_Cobrador").Value = ORCT.U_Cobrador;
                oIncomingPayments.UserFields.Fields.Item("U_AplicaRetencion").Value = "NO";
                oIncomingPayments.UserFields.Fields.Item("U_DpsBoleta").Value = ORCT.U_DpsBoleta;
                oIncomingPayments.UserFields.Fields.Item("U_BcoDpsBoleta").Value = ORCT.U_BcoDpsBoleta;


                // Agregar métodos de pago (EF, CH, TR)
                foreach (IncomingPaymentMethod pay in ORCT.IncomingPaymentMethod ?? new List<IncomingPaymentMethod>())
                {
                    if (pay.Type == "CH") // Pago con cheque
                    {
                        oIncomingPayments.Checks.CheckSum = double.Parse(pay.Sum);
                        oIncomingPayments.Checks.CheckNumber = Convert.ToInt32(pay.ReferenceNumber);
                        oIncomingPayments.Checks.CountryCode = pay.CountryCode;
                        oIncomingPayments.Checks.BankCode = pay.BankCode;
                        oIncomingPayments.Checks.Add();
                    }

                    if (pay.Type == "TR") // Pago por transferencia
                    {
                        oIncomingPayments.TransferSum += double.Parse(pay.Sum);
                        oIncomingPayments.TransferDate = DateTime.Parse(pay.Date);
                        oIncomingPayments.TransferReference = pay.ReferenceNumber;
                    }

                    if (pay.Type == "EF") // Pago en efectivo
                    {
                        oIncomingPayments.CashSum += double.Parse(pay.Sum); // Agregar el monto al campo CashSum
                    }
                }

                // Intentar crear el pago en SAP
                if (oIncomingPayments.Add() == 0)
                {
                    // Obtener el docEntry del pago creado
                    var docEntry = company.GetNewObjectKey();
                    Payments PagoCreado = (Payments)company.GetBusinessObject(BoObjectTypes.oIncomingPayments);

                    // Validar si el pago fue creado correctamente
                    _ = int.TryParse(docEntry, out int docEntry2);
                    if (PagoCreado.GetByKey(docEntry2))
                    {
                        return Ok(new MessageAPI() { Result = "OK", Message = "Creado Correctamente.", Code = PagoCreado.DocNum.ToString(), CodeNum = PagoCreado.CounterReference.ToString() });
                    }
                    else
                    {
                        return Conflict(new MessageAPI() { Result = "Fail", Message = "No se terminó el proceso correctamente", Code = string.Empty });
                    }
                }
                else
                {
                    company.GetLastError(out int errCode, out string errMsg);
                    return Conflict(new MessageAPI() { Result = "Fail", Message = errMsg, Code = string.Empty });
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el depósito - error: " + ex.Message });
            }

        }
    }
}

