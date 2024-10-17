using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using CanellaMovilBackend.Models.SAPModels;
using ConexionesSQL.Models;
using System.Data;
using CanellaMovilBackend.Filters.UserFilter;
using ConexionesSQL.SAP;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de entregas de venta
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(BlockEndpointFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class DeliveryController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public DeliveryController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Cierra las entregas abiertas
        /// </summary>
        /// <returns>Devuelve las entregas cerradas</returns>
        /// <param name="listOpenDelivery">Listado de entregas a cerrar</param>
        /// <response code="200">Entregas cerradas</response>
        /// <response code="409">No se cerraron entregas</response>
        [HttpPost]
        [ProducesResponseType(typeof(ListOpenDelivery), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CloseDelivery(ListOpenDelivery listOpenDelivery)
        {
            SP_DELI deli = new();
            var parameters = new List<Parametros>
            {
                new("Usuario", "apinfo7")
            };

            try
            {
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company oCompany = companyConnection.Company;

                // Acceso al transaction por usuario
                deli.DELI_AcceptCloseDelivery(parameters);

                foreach (OpenDelivery openDelivery in listOpenDelivery?.ListDelivery ?? [])
                {
                    // Obtener el objeto DeliveryNotes de la API de DI
                    Documents oDeliveryNotes = (Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
                    string comment = $"{listOpenDelivery?.Comment ?? ""}: AUDITORIA {oCompany.UserName} REALIZADO EL {DateTime.Now:dd-MM-yyyy}";

                    // Actualiza el comentario de la entrega
                    var parametersSelect = new List<Parametros>
                    {
                        new("DocNum", openDelivery.DocNum)
                    };
                    
                    var resultado = deli.DELI_SelectEntrega(parametersSelect);

                    if (resultado.Datos.Rows.Count > 0)
                    {
                        DataRow? dataRow = resultado.Datos?.Rows[0] ?? null;
                        _ = int.TryParse(dataRow?["DocEntry"]?.ToString() ?? "0", out int DocEntry);
                        openDelivery.DocEntry = DocEntry;
                    }

                    if (oDeliveryNotes.GetByKey(openDelivery.DocEntry) == true)
                    {
                        int result = oDeliveryNotes.Close();
                        oCompany.GetLastError(out int nErr, out string errMsgU_Cobrador);

                        if (result != 0)
                        {
                            openDelivery.Message = $"Error: {oCompany.GetLastErrorDescription()}";
                            openDelivery.State = StateDelivery.Error;
                        }
                        else
                        {
                            // Actualiza el comentario de la entrega
                            var parametersUpdate = new List<Parametros>
                            {
                                new("DocNum", openDelivery.DocNum),
                                new("Comentario", comment)
                            };
                            deli.DELI_ActualizarComentarioEntrega(parametersUpdate);

                            openDelivery.Message = "Cerrada";
                            openDelivery.State = StateDelivery.Close;
                        }
                    }
                    else
                    {
                        openDelivery.Message = "La entrega no existe";
                        openDelivery.State = StateDelivery.Error;
                    }
                }
                // Deniega el acceso al transaction por usuario
                deli.DELI_DenyCloseDelivery(parameters);

                return Ok(listOpenDelivery);
            }
            catch(Exception ex)
            {
                // Deniega el acceso al transaction por usuario
                deli.DELI_DenyCloseDelivery(parameters);

                return Conflict(new MessageAPI() { Message = ex.Message });
            }
        }
    }
}
