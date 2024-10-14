using CanellaMovilBackend.Filters.UserFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Models;
using ConexionesSQL.Models;
using ConexionesSQL;
using CanellaMovilBackend.Models.PowerAutomateModels;

namespace CanellaMovilBackend.Controllers.PowerAutomateControllers
{
    /// <summary>
    /// Controlador de Confirmación de boletas
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class BoletasController : ControllerBase
    {
        /// <summary>
        /// Actualiza el estado de confirmación de boletas
        /// </summary>
        /// <returns>Devuelve las entregas cerradas</returns>
        /// <param name="confirmarBoleta">Objecto que envia los datos para actualizar</param>
        /// <response code="200">Actaulización correcta</response>
        /// <response code="409">Error al actualizar boleta</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageBoleta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult ConfirmacionBoleta(ConfirmarBoleta confirmarBoleta)
        {
            try
            {
                SP_AUTOMATE deli = new();
                var parameters = new List<Parametros>
                {
                    new("@id", confirmarBoleta.IdSTOD),
                    new("@Estado", confirmarBoleta.Estado),
                    new("@IdPowerAutomate", confirmarBoleta.ID)
                };

                var resultado = deli.AUTOMATE_UpdateEstadoConfirmacionBoletas(parameters);
                if (resultado.MensajeTipo == 1)
                {
                    return Ok(new MessageBoleta { status = "Actualizado correctamente." });
                }
                else
                {
                    return Conflict(new MessageAPI() { Message = resultado.MensajeDescripcion });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Message = ex.Message });
            }
        }
        /// <summary>
        /// Actualiza el estado de confirmación de boletas
        /// </summary>
        /// <returns>Devuelve las entregas cerradas</returns>
        /// <param name="consultarRepuestos">Objecto que envia los datos para actualizar</param>
        /// <response code="200">Actaulización correcta</response>
        /// <response code="409">Error al actualizar boleta</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageBoleta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult ConsultarRepuestosIsuzu(ConsultarRepuestos consultarRepuestos)
        {
            try
            {
                SP_AUTOMATE deli = new();
                var parameters = new List<Parametros>
                {
                    new("@id", consultarRepuestos.IdSTOD),
                    new("@PiezaDisponible", consultarRepuestos.PiezaDisponible),
                    new("@Descripcion", consultarRepuestos.Descripcion),
                    new("@UnidadesPorPaquete", consultarRepuestos.UnidadesPorPaquete),
                    new("@ExistenciasEnFabrica", consultarRepuestos.ExistenciasEnFabrica),
                    new("@Precio", consultarRepuestos.Precio)
                };

                var resultado = deli.AUTOMATE__CRI_UpdateDatosConsultaRepuestos(parameters);
                if (resultado.MensajeTipo == 1)
                {
                    return Ok(new MessageBoleta { status = "Actualizado correctamente." });
                }
                else
                {
                    return Conflict(new MessageAPI() { Message = resultado.MensajeDescripcion });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Message = ex.Message });
            }
        }
    }
}
