using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.Journal_Entry;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de Asientos Contables
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class JournalEntry : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public JournalEntry(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Crea un asiento contable en SAP
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreateJournalEntry(OJDT OJDT)
        {
            try
            {
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;

                JournalEntries journalEntries = (JournalEntries)company.GetBusinessObject(BoObjectTypes.oJournalEntries);
                journalEntries.ReferenceDate = DateTime.Parse(OJDT.RefDate);
                journalEntries.DueDate = DateTime.Parse(OJDT.DueDate);
                journalEntries.TaxDate = DateTime.Parse(OJDT.TaxDate);
                journalEntries.Memo = OJDT.Memo;
                journalEntries.Reference = OJDT.Ref1;
                journalEntries.Reference2 = OJDT.Ref2;
                journalEntries.TransactionCode = OJDT.TransCode;
                journalEntries.Reference3 = OJDT.Ref3;

                foreach (JDT1 journalLine in OJDT.JournalLine ?? [])
                {
                    journalEntries.Lines.AccountCode = journalLine.Account;
                    journalEntries.Lines.Debit = double.Parse(journalLine.Debit);
                    journalEntries.Lines.Credit = double.Parse(journalLine.Credit);
                    journalEntries.Lines.LineMemo = journalLine.LineMemo;
                    journalEntries.Lines.Add();
                }

                journalEntries.Add();

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
    }
}
