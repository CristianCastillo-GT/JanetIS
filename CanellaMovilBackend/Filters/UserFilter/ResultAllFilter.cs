using CanellaMovilBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CanellaMovilBackend.Service.UserService;
using ConexionesSQL;
using ConexionesSQL.Models;
using CanellaMovilBackend.Utils;
using Newtonsoft.Json;
using System;

namespace CanellaMovilBackend.Filters.UserFilter
{

    /// <summary>
    /// Filtro por resultado
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    public class ResultAllFilter(IUserService userService) : ResultFilterAttribute
    {
        private readonly IUserService userService = userService;

        /// <summary>
        /// Ejecución del resultado del filtro
        /// </summary>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            AuthenticationTokenUser authenticationToken = userService.GetAuthenticationToken();

            LogsAPI.Info("-- Resultado");
            try
            {
                LogsAPI.Info($"Usuario: {authenticationToken.UserId}, " +
                    $"Role: {authenticationToken.Role}, " +
                    $"Action: {context.RouteData.Values["action"]}, " +
                    $"Controller: {context.RouteData.Values["controller"]}," +
                    $"StatusCode: {context.HttpContext.Response.StatusCode}, " +
                    $"ResultObject: {JsonConvert.SerializeObject(context.Result)}");
                LogsAPI.Info("---------------");
            }
            catch (Exception ex)
            {
                LogsAPI.Info(ex.Message);
            }
            return;
        }
    }
}
