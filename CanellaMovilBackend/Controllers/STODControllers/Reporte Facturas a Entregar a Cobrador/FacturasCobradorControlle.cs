using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Utils;
using ConexionesSQL.Models;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models.STODModels.Reporte_Facturas_a_Entregar_a_Cobrador;
using CanellaMovilBackend.Service.UserService;
using ConexionesSQL.STOD.Reporte_Facturas_a_Entregar_a_Cobrador;

namespace CanellaMovilBackend.Models.STODModels.Reporte_Facturas_a_Entregar_a_Cobrador
{
    /// <summary>
    /// Controlador para el Reporte de facturas a entregar a Cobrador de stod
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]

    public class FacturasCobradorController() : ControllerBase
    {
        /// <summary>
        /// Se obtienen las facturas para Entregar a Cobrador
        /// </summary>
        /// <param name="Fecha">Fecha</param>
        /// <param name="Serie">Factura Serie (T = Todas)</param>
        /// <param name="Cobrador">Cobrador (T = Todos)</param>
        /// <returns>Retorna un listado de las ventas por gama</returns>
        /// <response code="200">obtención de datos exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpGet]
        [ProducesResponseType(typeof(Facturas_A_Cobrador), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult FacturaCreditoCanella(DateTime Fecha, string Serie, string Cobrador)
        {
            try
            {
              
                var parameters = new List<Parametros>()
                {
                    new("@Fecha", Fecha),
                    new("@Serie", Serie),
                    new("@Cobrador", Cobrador)
                };

                SP_OFC sp_OFC = new();
                var resultado = sp_OFC.OFC_ObtenerFacturas_Creditos(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    return Ok(ObjectConvert.CreateListFromDataTable<Facturas_A_Cobrador>(resultado.Datos));
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
        /// Se obtienen las facturas para Entregar a Cobrador
        /// </summary>
        /// <param name="Fecha">Fecha</param>
        /// <param name="Cobrador">Cobrador (T = Todos)</param>
        /// <returns>Retorna un listado de las ventas por gama</returns>
        /// <response code="200">obtención de datos exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpGet]
        [ProducesResponseType(typeof(Facturas_A_Cobrador), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult FacturaCreditoVesa(DateTime Fecha, string Cobrador)
        {
            try
            {

                var parameters = new List<Parametros>()
                {
                    new("@Fecha", Fecha),
                    new("@Cobrador", Cobrador)
                };

                SP_OFC sp_OFC = new();
                var resultado = sp_OFC.OFC_ObtenerFacturas_CreditosVesa(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    return Ok(ObjectConvert.CreateListFromDataTable<Facturas_A_Cobrador>(resultado.Datos));
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
        /// Se obtienen las facturas para Entregar a Cobrador
        /// </summary>
        /// <param name="Fecha">Fecha</param>
        /// <param name="Cobrador">Cobrador (T = Todos)</param>
        /// <returns>Retorna un listado de las ventas por gama</returns>
        /// <response code="200">obtención de datos exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpGet]
        [ProducesResponseType(typeof(Facturas_A_Cobrador), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult FacturaCreditoMaquipos(DateTime Fecha, string Cobrador)
        {
            try
            {

                var parameters = new List<Parametros>()
                {
                    new("@Fecha", Fecha),
                    new("@Cobrador", Cobrador)
                };

                SP_OFC sp_OFC = new();
                var resultado = sp_OFC.OFC_ObtenerFacturas_CreditosVesaMaquipos(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    return Ok(ObjectConvert.CreateListFromDataTable<Facturas_A_Cobrador>(resultado.Datos));
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

