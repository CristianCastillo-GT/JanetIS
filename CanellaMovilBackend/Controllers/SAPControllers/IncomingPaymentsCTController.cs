using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.IncomingPayments;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using System.Data.SqlClient;
using System.Data;
using ConexionesSQL.STOD.RECI;
using ConexionesSQL.Models;
using ConexionesSQL.STOD.Dashboard;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;
using CanellaMovilBackend.Models.STODModels.Dashboard;
using CanellaMovilBackend.Utils;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    ///  Controladore Pagos Recibidos CT
    /// </summary>
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    //[ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class IncomingPaymentsCTController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>

        public IncomingPaymentsCTController(ISAPService sapService) 
        { 
            this.sapService = sapService;
        }

        /// <summary>
        /// Crea pagos recibidos en SAP con Tarjeta Credito
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creacion exitosa</response>
        /// <response code="409">Creacion exitosa</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<MessageAPI>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult BulkCReatePaymentCT(List<ORCT> ORCTList)
        {
            try
            {
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;
                List<MessageAPI> messageApi = [];


                foreach (ORCT ORCT in ORCTList)
                {
                    try
                    {
                        Payments? oIncomingPayments = (Payments)company.GetBusinessObject(BoObjectTypes.oIncomingPayments);

                        oIncomingPayments.DocType = (ORCT.DocType == "A" ? BoRcptTypes.rAccount : (ORCT.DocType == "C" ? BoRcptTypes.rCustomer : (ORCT.DocType == "S" ? BoRcptTypes.rSupplier : throw new InvalidOperationException("El tipo de pago no es válido"))));
                        oIncomingPayments.DocDate = DateTime.Parse(ORCT.DocDate);
                        oIncomingPayments.DueDate = DateTime.Parse(ORCT.DocDueDate);
                        oIncomingPayments.TaxDate = DateTime.Parse(ORCT.TaxDate);
                        oIncomingPayments.CardCode = ORCT.CardCode;
                        oIncomingPayments.CardName = ORCT.CardName;
                        oIncomingPayments.UserFields.Fields.Item("U_TipoPago").Value = "3";
                        oIncomingPayments.UserFields.Fields.Item("U_TipoPagos").Value = "3";
                        oIncomingPayments.ControlAccount = "_SYS00000000552";
                        oIncomingPayments.Series = 221;
                        oIncomingPayments.Remarks = ORCT.Comments;

                        DataTable ohem = getCentroCosto(ORCT.U_EmpCode.ToString());
                        if (ohem != null)
                        {
                            oIncomingPayments.UserFields.Fields.Item("U_Ceco").Value = ohem.Rows[0]["CentroCosto"]?.ToString() ?? "";
                            oIncomingPayments.UserFields.Fields.Item("U_EmpVentas").Value = ohem.Rows[0]["EmpleadoVenta"]?.ToString() ?? "";
                            oIncomingPayments.UserFields.Fields.Item("U_CecoNombre").Value = ohem.Rows[0]["NombreCentroCosto"]?.ToString() ?? "";
                            oIncomingPayments.UserFields.Fields.Item("U_EmpCode").Value = ORCT.U_EmpCode;
                        }

                        foreach (RCT3 card in ORCT.RCT3 ?? new List<RCT3>())
                        {
                            oIncomingPayments.CreditCards.CreditCard = int.Parse(card.CreditCard);
                            oIncomingPayments.CreditCards.CreditCardNumber = card.CrCardNum;
                            oIncomingPayments.CreditCards.CardValidUntil = new DateTime(Convert.ToInt32(card.CardValidUntil.ToString().Substring(2, 2)) + 2000, Convert.ToInt32(card.CardValidUntil.ToString().Substring(0, 2)), 1); // Fecha de expiración
                            oIncomingPayments.CreditCards.CreditSum = double.Parse(card.SumPaid); // Monto pagado
                            oIncomingPayments.CreditCards.VoucherNum = card.VoucherNum; // Número de voucher o recibo

                            // Agregar línea de tarjeta de crédito
                            oIncomingPayments.CreditCards.Add();
                        }

                        foreach (var invoice in ORCT.INV ?? new List<INV>())
                        {
                            int docEntry = GetInvoiceDocEntryByNumAtCard(company, invoice.NumAtCard);
                            if (docEntry > 0)
                            {
                                oIncomingPayments.Invoices.DocEntry = docEntry; // ID de la factura
                                oIncomingPayments.Invoices.SumApplied = double.Parse(invoice.SumApplied); // Monto aplicado
                                oIncomingPayments.Invoices.Add(); // Añadir la factura
                            }
                        }

                        if (oIncomingPayments.Add() == 0)
                        {
                            var docentry = company.GetNewObjectKey();
                            Payments PagoCreado = (Payments)company.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                            _ = int.TryParse(docentry, out int docentry2);

                            if (PagoCreado.GetByKey(docentry2))
                            {
                                ORCT.MensajeExito = "Se creo El pago a Cuenta: "+ PagoCreado.DocNum.ToString();
                            }
                            else
                            { 
                                ORCT.ErrorSAP =  "No se pudo obtener el pago recibido: " + docentry2;
                            }
                        }
                        else
                        {
                            company.GetLastError(out int errCode, out string errMsg);
                            ORCT.ErrorSAP = errMsg;
                        }
                    }
                    catch (Exception ex) 
                    { 
                        ORCT.ErrorSAP = ex.Message;
                        
                    }
                }
                return Ok(ORCTList.Select(x => new { x.CardCode, x.CardName, CreditCards = x.RCT3?.Select(
                    card => new { card.CreditCard, card.CrCardNum, card.SumPaid }), x.ErrorSAP, x.MensajeExito}));
            }
            catch (Exception ex) 
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el pago - error: " + ex.Message });
            }
        }
        private DataTable? getCentroCosto(string codigoVendedor)
        {
            var parameters = new List<Parametros>()
            {
                    new("@CodigoVendedor", codigoVendedor)
                };

            RECI CodVendedor = new();
            var resultado = CodVendedor.API_RECI_CodigoEmpleado(parameters);

            if (resultado.MensajeTipo == 1)
            {
                return resultado.Datos;
            }
            else
            {
                return null;
            }
           
        }

        private int GetInvoiceDocEntryByNumAtCard(Company company, string numAtCard)
        {
            Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string query = $"SELECT DocEntry FROM OINV WHERE NumAtCard = '{numAtCard}'";
            recordset.DoQuery(query);

            if (!recordset.EoF)
            {
                return Convert.ToInt32(recordset.Fields.Item("DocEntry").Value);
            }

            return 0; // Si no se encuentra la factura
        }
    }
}
