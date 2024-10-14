using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Utils;
using ConexionesSQL.Models;
using CanellaMovilBackend.Filters.UserFilter;
using ConexionesSQL.STOD.Dashboard;
using CanellaMovilBackend.Models.STODModels.Dashboard;
using CanellaMovilBackend.Service.UserService;

namespace CanellaMovilBackend.Controllers.STODControllers.Dashboard
{
    /// <summary>
    /// Controlador para el dashboard de stod
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="userService"></param>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(STODFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class DashboardController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Se obtienen las ventas anuales o mensuales por gama y marca
        /// </summary>
        /// <param name="Marca">Se manda la marca a obtener</param>
        /// <param name="Mensual">Indica si se obtienen las ventas mes actual o anual true=Mes actual, false=Anual</param>
        /// <returns>Retorna un listado de las ventas por gama</returns>
        /// <response code="200">obtención de datos exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpGet]
        [ProducesResponseType(typeof(GamaVentas), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult VentaGama(string Marca, bool Mensual)
        {
            try
            {
                AuthenticationTokenUser authenticationToken = userService.GetAuthenticationToken();

                var parameters = new List<Parametros>()
                {
                    new("@Marca", Marca),
                    new("@Mensual", Mensual),
                    new("@Usuario", authenticationToken.Username)
                };

                SP_MOVV sp_MOVV = new();
                var resultado = sp_MOVV.MOVV_DashboardGamaMarca(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    return Ok(ObjectConvert.CreateListFromDataTable<GamaVentas>(resultado.Datos));
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = resultado.MensajeDescripcion });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = ex.Message });
            }
        }

        /// <summary>
        /// Se obtienen los nuevos clientes del año actual y del mes actual
        /// </summary>
        /// <param name="Marca">Se manda la marca a obtener</param>
        /// <returns>Retorna los clientes nuevos y sus metas</returns>
        /// <response code="200">obtención de datos exitosa</response>
        /// <response code="404">No se encontro información</response>
        /// <response code="409">Mensaje de error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ClientesNuevos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult ClientesNuevos(string Marca)
        {
            try
            {
                AuthenticationTokenUser authenticationToken = userService.GetAuthenticationToken();

                var parameters = new List<Parametros>()
                {
                    new("@Marca", Marca),
                    new("@Usuario", authenticationToken.Username)
                };

                SP_MOVV sp_MOVV = new();
                var resultado = sp_MOVV.MOVV_DashboardNuevosClientes(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    var data = ObjectConvert.CreateListFromDataTable<ClientesNuevos>(resultado.Datos);
                    if (data.Count > 0)
                        return Ok(data.First());
                    else
                        return NotFound(new MessageAPI() { Result = "Fail", Message = "No se encontraron clientes nuevos" });
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = resultado.MensajeDescripcion });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = ex.Message });
            }
        }

        /// <summary>
        /// Se obtienen las ventas mensuales anterior al mes actual por marca
        /// </summary>
        /// <param name="Marca">Se manda la marca a obtener</param>
        /// <returns>Retorna un listado de las ventas por gama</returns>
        /// <response code="200">obtención de datos exitosa</response>
        /// <response code="404">No se encontraron datos</response>
        /// <response code="409">Mensaje de error</response>
        [HttpGet]
        [ProducesResponseType(typeof(GamaVentasMesAnterior), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult VentaGamaMesAnterior(string Marca)
        {
            try
            {
                AuthenticationTokenUser authenticationToken = userService.GetAuthenticationToken();

                var parameters = new List<Parametros>()
                {
                    new("@Marca", Marca),
                    new("@Usuario", authenticationToken.Username)
                };

                SP_MOVV sp_MOVV = new();
                var resultado = sp_MOVV.MOVV_DashboardGamaMarcaMesAnterior(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    List<GamaVentasMesAnterior> lista = ObjectConvert.CreateListFromDataTable<GamaVentasMesAnterior>(resultado.Datos);
                    if (lista.Count > 0)
                    {
                        return Ok(lista.First());
                    }
                    return NotFound(new MessageAPI() { Result = "Fail", Message = "No se encontro ventas del mes anterior"});
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = resultado.MensajeDescripcion });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = ex.Message });
            }
        }
    }
}
