using CanellaMovilBackend.Models;
using CanellaMovilBackend.Service.UserService;
using CanellaMovilBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace CanellaMovilBackend.Filters.UserFilter
{
    /// <summary>
    /// Filtro para comprobar usuario de STOD
    /// </summary>
    public class STODFilter : ActionFilterAttribute
    {
        private readonly IUserService userService;

        /// <summary>
        /// Constructor
        /// </summary>
        public STODFilter(IUserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// Ejecución de la accion del filtro
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AuthenticationTokenUser authenticationToken = userService.GetAuthenticationToken();

            LogsAPI.Info("--");
            LogsAPI.Info($"Usuario: {authenticationToken.UserId}, Role: {authenticationToken.Role}, Action: {context.RouteData.Values["action"]}, Controller: {context.RouteData.Values["controller"]}");

            if (context.HttpContext.Request.Headers.ContainsKey("STODNET") && context.HttpContext.Request.Headers.TryGetValue("STODNET", out StringValues value) && value.ToString() == "true")
            {
                return;
            }
            else
            {
                var message = $"El usuario {authenticationToken.Username} con rol {authenticationToken.Role} no tiene autorización a esta acción del STOD";
                LogsAPI.Info(message);
                context.Result = new UnauthorizedObjectResult(new MessageAPI() { Message = message });
                return;
            }
        }
    }
}
