using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.SAPModels.Deposit;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de Depósitos
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class DepositsServiceController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public DepositsServiceController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Crea un deposito en SAP
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreateDeposit(List<ODPS> ODPSList)
        {
            Company company = sapService.SAPB1();
            List<MessageAPI> result = [];
            try
            {
                DepositsService oDepositsService = (DepositsService)company.GetCompanyService().GetBusinessService(ServiceTypes.DepositsService);
                DepositParams? DEPOSIT = null;

                foreach (ODPS ODPS in ODPSList ?? [])
                {

                    Deposit oDeposit = (Deposit)oDepositsService.GetDataInterface(DepositsServiceDataInterfaces.dsDeposit);

                    // Encabezado principal del deposito
                    oDeposit.DepositDate = DateTime.Parse(ODPS.DeposDate);
                    oDeposit.DepositCurrency = ODPS.DeposCurr;
                    oDeposit.DepositAccount = ODPS.BanckAcct;
                    oDeposit.BankBranch = ODPS.Ref2;
                    oDeposit.BankReference = ODPS.Ref2.Contains('-') ? ODPS.Ref2[..ODPS.Ref2.IndexOf('-')] : ODPS.Ref2;
                    oDeposit.JournalRemarks = ODPS.Memo;

                    if (ODPS.DepositType == "cash")
                    {
                        // Necesario para deposito de tipo CASH
                        oDeposit.DepositType = BoDepositTypeEnum.dtCash;
                        oDeposit.AllocationAccount = ODPS.AllocAcct;
                        oDeposit.TotalLC = Convert.ToDouble(ODPS.LocTotal);
                        oDeposit.DocRate = Convert.ToDouble(ODPS.DocRate);
                    }
                    if (ODPS.DepositType == "credit")
                    {
                        // Necesario para deposito de tipo Credit Card                
                        oDeposit.DepositType = BoDepositTypeEnum.dtCredit;
                        oDeposit.DepositAccountType = BoDepositAccountTypeEnum.datBankAccount;
                        oDeposit.VoucherAccount = ODPS.CrdBankAct;
                        oDeposit.CommissionAccount = ODPS.CommissionAccount;
                        oDeposit.Commission = Convert.ToDouble(ODPS.Commision);
                        oDeposit.TaxAccount = ODPS.TaxAccount;
                        oDeposit.TaxAmount = Convert.ToDouble(ODPS.TaxAmount);
                        oDeposit.CommissionDate = DateTime.Parse(ODPS.CommissionDate);
                        oDeposit.BankAccountNum = ODPS.DeposAcct;

                        foreach (string absid in ODPS?.AbsId ?? [])
                        {
                            CreditLines credits = oDeposit.Credits;
                            CreditLine credit;
                            credit = credits.Add();
                            credit.AbsId = Convert.ToInt32(absid);
                        }
                    }

                    try
                    {
                        //Agregar el nuevo registro a la tabla ODPS
                        DEPOSIT = oDepositsService.AddDeposit(oDeposit);
                    }
                    catch (Exception ex)
                    {
                        result?.Add(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el deposito - error: " + ex.Message });
                    }

                    if (DEPOSIT != null)
                    {
                        result?.Add(new MessageAPI() { Result = "OK", Message = "Se creo el registro con el numero de deposito: " + DEPOSIT?.DepositNumber, Code = ODPS?.Ref2 ?? "" });
                        DEPOSIT = null;
                    }
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                result?.Add(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el deposito - error: " + ex.Message });
            }
            return Ok(result);
        }
    }
}
