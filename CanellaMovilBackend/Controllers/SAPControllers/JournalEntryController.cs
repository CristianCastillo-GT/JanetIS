using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.Journal_Entry;
using CanellaMovilBackend.Models.SAPModels.TeamCard;
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
       // [ApiExplorerSettings(IgnoreApi = true)]
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


        /// <summary>
        /// Actualiza un asiento contable en SAP
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult UpdateJournal(OJDT oJDT)
        {
            try
            {
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;

                // Crear objeto JournalEntries
                JournalEntries journalEntrie = (JournalEntries)company.GetBusinessObject(BoObjectTypes.oJournalEntries);

                // Buscar el JournalEntry por su número
                if (!journalEntrie.GetByKey(oJDT.Number))
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se encontró el Asiento Contable con el número proporcionado."+ oJDT.Number });
                }

                bool isUpdated = false;

                // Iterar por las líneas del JournalEntry
                foreach (JDT1 journalLine in oJDT.JournalLine ?? [])
                {
                    for (int i = 0; i < journalEntrie.Lines.Count; i++)
                    {
                        journalEntrie.Lines.SetCurrentLine(i);

                        // Verificar el monto y el CostingCode
                        bool matchesDebit = journalLine.Amount < 0 && Math.Abs(journalEntrie.Lines.Debit - Math.Abs(journalLine.Amount)) < 0.01;
                        bool matchesCredit = journalLine.Amount > 0 && Math.Abs(journalEntrie.Lines.Credit - journalLine.Amount) < 0.01;

                        if ((matchesDebit || matchesCredit) && journalEntrie.Lines.CostingCode == journalLine.CostingCode)
                        {
                            // Actualizar CostingCode
                            journalEntrie.Lines.CostingCode = journalLine.NewCostingCode;
                            isUpdated = true;
                        }
                    }
                }

                // Si se realizó alguna actualización, guardar los cambios
                if (isUpdated)
                {
                    if (journalEntrie.Update() != 0)
                    {
                        return Conflict(new MessageAPI()
                        {
                            Result = "Fail",
                            Message = "Error actualizando el Asiento Contable: " + company.GetLastErrorDescription()
                        });
                    }
                    return Ok(new MessageAPI() { Result = "OK", Message = "Centro de Costo actualizado correctamente." });
                }

                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se encontraron líneas para actualizar." });



            }
            catch (Exception ex) 
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo Actualizar Asiento Contable - error: " + ex.Message });
            }
        }

    }
}
