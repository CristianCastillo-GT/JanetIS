using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.SAPModels.Human_Resources;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de Recursos Humanos en SAP
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class HumanResourcesController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public HumanResourcesController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Obtiene el listado total de vendedores en SAP
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<OSLP>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetSalesPerson()
        {
            Company company = sapService.SAPB1();
            try
            {
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery("select SlpCode, SlpName, U_Email from OSLP WHERE Active = 'Y' AND SlpCode != '-1'");
                if (recordset.RecordCount > 0)
                {
                    List<OSLP> ListSalesPerson = [];
                    while (!recordset.EoF)
                    {
                        OSLP SalesPerson = new()
                        {
                            SlpCode = Convert.ToString(recordset.Fields.Item("SlpCode").Value),
                            SlpName = (string)recordset.Fields.Item("SlpName").Value,
                            U_Email = Convert.ToString(recordset.Fields.Item("U_Email").Value)
                        };

                        ListSalesPerson.Add(SalesPerson);
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(ListSalesPerson);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(new MessageAPI() { Result = "OK", Message ="No se encontró ningun registro"  });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de vendedores - " + ex.Message });
            }
        }
    }
}
