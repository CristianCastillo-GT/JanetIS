using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.SAPModels.User_Defined_Tables;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador del socio de negocios
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class UserDefinedTableController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public UserDefinedTableController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Obtiene el listado total de cobradores de SAP
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Cobrador>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetDebtCollectors(int empresa)
        {
            Company company = sapService.SAPB1();
            try
            {
                //CompanyConnection companyConnection = sapService.SAPB1();
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

                // Obtener el objeto @COBRADOR de la API de DI
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string consulta;

                switch (empresa)
                {
                    case 1:
                        consulta = "SELECT Code, Name FROM [dbo].[@COBRADORES]";
                        break; // Asegúrate de usar break aquí para evitar caer en el siguiente caso

                    case 2:
                        consulta = $"SELECT Code, Name FROM [{configuration.GetConnectionString("VESA") ?? ""}].[SBO_VESA].[dbo].[@COBRADORES]";
                        break;

                    case 3:
                        consulta = $"SELECT Code, Name FROM [{configuration.GetConnectionString("TALLER") ?? ""}].[TALLER].[dbo].[@COBRADORES]";
                        break;

                    case 4:
                         consulta = $"SELECT Code, Name FROM [{configuration.GetConnectionString("MAQUIPOS") ?? ""}].[SBO_MAQUIPOS].[dbo].[@CBRD]";
                        break;

                    default:
                    {
                            sapService.SAPB1_DISCONNECT(company);
                            return Conflict(new MessageAPI() { Result = "Fail", Message = "Empresa no válida" });
                    }
                }

                // Ejecutar la consulta
                recordset.DoQuery(consulta);
                if (recordset.RecordCount > 0)
                {
                    List<Cobrador> ListCobradores = [];

                    while (!recordset.EoF)
                    {
                        Cobrador cobrador = new()
                        {
                            Code = (string)recordset.Fields.Item("Code").Value,
                            Name = (string)recordset.Fields.Item("Name").Value,
                        };

                        ListCobradores.Add(cobrador);
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(ListCobradores);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return NotFound(new MessageAPI() { Result = "OK", Message ="No se encontró ningun registro"  });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de cobradores - " + ex.Message });
            }
        }
    }
}
