using CanellaMovilBackend.Filters.UserFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.SAPModels.Reports;
using ConexionesSQL.SAP;
using ConexionesSQL.Models;
using CanellaMovilBackend.Utils;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de Repuestos
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class ReplacementCallController : ControllerBase
    {
        /// <summary>
        /// Obtiene el registro por Número Artículo
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpGet]
        [ProducesResponseType(typeof(ListadoArticulos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult SearchPrecioArticulo(string ItemCode)
        {
            try
            {
                var parameters = new List<Parametros>()
                    {
                    new("@ItemCode", ItemCode)
                };

                SP_CNAR replacementCall = new();

                var resultado = replacementCall.CRCO_SelectPrecioRepuesto(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    return Ok(ObjectConvert.CreateListFromDataTable<ListadoArticulos>(resultado.Datos));
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
        /// Obtiene los repuestos instalados al equipo por medio del Número de Serie 
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpGet]
        [ProducesResponseType(typeof(RepuestosInstalados), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult SearchRepuestosInstalados(string U_Chasis)
        {
            try
            {
                var parameters = new List<Parametros>()
                    {
                    new("@U_Chasis", U_Chasis)
                };

                SP_CNAR replacementInstall = new();

                var resultado = replacementInstall.CRCS_SelectRepuestoNumSerie(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    return Ok(ObjectConvert.CreateListFromDataTable<RepuestosInstalados>(resultado.Datos));
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
