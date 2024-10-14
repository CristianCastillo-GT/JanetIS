using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.SAPModels.Bank_Statements_and_External_Reconciliations;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Modelo para la trata de estados de cuenta externos
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class ExternalBankStatementController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public ExternalBankStatementController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Crea un registro en el extracto bancario
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<MessageAPI>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreateBnkRecord(List<OBNK> OBNKList)
        {
            Company company = sapService.SAPB1();
            try
            {
                BankPages bnkPage = company.GetBusinessObject(BoObjectTypes.oBankPages);
                List<MessageAPI> ListMessage = [];

                foreach (OBNK OBNK in OBNKList.Where(x => DateTime.Compare(DateTime.ParseExact(x.DueDate, "dd/MM/yyyy", null), DateTime.Today) < 0) ?? [])
                {
                    try
                    {
                        bnkPage.AccountCode = OBNK.AcctCode;
                        bnkPage.DueDate = DateTime.Parse(OBNK.DueDate);
                        bnkPage.Reference = OBNK.Ref;
                        bnkPage.Memo = OBNK.Memo;
                        _ = double.TryParse(OBNK.DebAmount?.Replace(",", ".") ?? "", out double debAmount);
                        _ = double.TryParse(OBNK.CredAmnt?.Replace(",", ".") ?? "", out double credAmnt);
                        bnkPage.DebitAmount = Math.Abs(debAmount);
                        bnkPage.CreditAmount = Math.Abs(credAmnt);

                        if (bnkPage.Add() == 0)
                            ListMessage.Add(new MessageAPI() { Result = "OK", Message = "Creado Correctamente.", Code = OBNK.Code });
                        else
                            ListMessage.Add(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el registro - error: " + company.GetLastErrorDescription(), Code = OBNK.Code });
                    }
                    catch (Exception ex)
                    {
                        ListMessage.Add(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el registro - error: " + ex.Message });
                    }
                }
                sapService.SAPB1_DISCONNECT(company);
                return Ok(ListMessage);
            }
            catch(Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear los registros - error: " + ex.Message });
            }
        }
    }
}
