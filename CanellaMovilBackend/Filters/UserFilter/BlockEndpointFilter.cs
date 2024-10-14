using CanellaMovilBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CanellaMovilBackend.Filters.UserFilter
{
    /// <summary>
    /// Filtro para bloquear Endpoints sin uso
    /// </summary>
    public class BlockEndpointFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Ejecución de la accion del filtro
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.Result = new UnauthorizedObjectResult(new MessageAPI() { Message = "Acceso denegado, endpoint no esta en uso" });
            return;
        }
    }
}
