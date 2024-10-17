using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CanellaMovilBackend.Filters
{
    /// <summary>
    /// Filtro de evaluación de conexión a SAP
    /// </summary>
    public class SAPConnectionFilter : ActionFilterAttribute
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SAPConnectionFilter(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Ejecución de la accion del filtro
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            CompanyConnection companyConnection = this.sapService.SAPB1();

            if (!(companyConnection.Connected))
            {
                context.Result = new BadRequestObjectResult(new MessageAPI() { Message = "Sin conexión a SAP " + companyConnection.Company.GetLastErrorDescription() });
                return;
            }
            return;
        }

    }
}
