using CanellaMovilBackend.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using CanellaMovilBackend.Service.UserService;
using CanellaMovilBackend.Utils;

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
                    $"StatusCode: {context.HttpContext.Response.StatusCode}, " 
                    //$"ResultObject: {JsonConvert.SerializeObject(context.Result)}"
                );
                LogsAPI.Info("---------------");
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
            catch (Exception ex)
            {
                LogsAPI.Info(ex.Message);
            }
            return;
        }
    }
}
