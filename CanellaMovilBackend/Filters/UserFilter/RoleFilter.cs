using CanellaMovilBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CanellaMovilBackend.Service.UserService;
using ConexionesSQL;
using ConexionesSQL.Models;
using CanellaMovilBackend.Utils;
using Newtonsoft.Json;

namespace CanellaMovilBackend.Filters.UserFilter
{

    /// <summary>
    /// Filtro por rol
    /// </summary>
    public class RoleFilter : ActionFilterAttribute
    {
        private readonly IUserService userService;

        /// <summary>
        /// Constructor
        /// </summary>
        public RoleFilter(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Ejecución de la accion del filtro
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AuthenticationTokenUser authenticationToken = userService.GetAuthenticationToken();

            SP_ROLE role = new();
            var parameters = new List<Parametros>
            {
                new("Role", authenticationToken.Role),
                new("Action", context.RouteData.Values["action"]),
                new("Controller", context.RouteData.Values["controller"])
            };

            LogsAPI.Info("--");
            LogsAPI.Info($"Usuario: {authenticationToken.UserId}, " +
                $"Role: {authenticationToken.Role}, " +
                $"Action: {context.RouteData.Values["action"]}, " +
                $"Controller: {context.RouteData.Values["controller"]}, " +
                $"Argumentos: {string.Join(";", context.ActionArguments?.Select(x =>
                {
                    var value = "";
                    if (x.Value?.GetType() == typeof(string))
                    {
                        value = x.Value?.ToString() ?? "";
                    }
                    else if (x.Value != null)
                    {
                        value = JsonConvert.SerializeObject(x.Value);
                    }
                    return ("Key=" + x.Key?.ToString() ?? "") + ",Value=" + value;
                }).ToList() ?? [])}");
            var resultado = role.ROLE_Permisos(parameters);

            if (resultado.Datos.Rows.Count == 0)
            {
                var message = $"El usuario {authenticationToken.Username} con rol {authenticationToken.Role} no tiene autorización a esta acción";
                LogsAPI.Info(message);
                context.Result = new ObjectResult(new MessageAPI() { Message = message }) { StatusCode = 403 };
                return;
            }
            LogsAPI.Info("Acceso concedido");
            return;
        }
    }
}
