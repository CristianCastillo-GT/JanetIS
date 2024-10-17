using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.PageCanon;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using static CanellaMovilBackend.Models.MotulTopKe.MotulRequestData;
using CanellaMovilBackend.Models.MotulTopKe;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models.PaginasWebModels;

namespace CanellaMovilBackend.Controllers.MotulTopKe
{
    /// <summary>
    /// Controlador de motul TopKe
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    //[ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class MotulTopKeController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        /// 
        public MotulTopKeController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Obtiene los datos de inventario de motul de sap canella
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<InventarioCDRioHondo>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetInventario(MotulRequestData.RequestGetInventarioMotul request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.clsEmpresa ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WSMotul_SELECT_INVENTORY_MOTUL_RIOHONDO '" + request.clsEmpresa + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<InventarioCDRioHondo> List_Inventario = [];
                    while (!recordsetUT.EoF)
                    {
                        InventarioCDRioHondo? code = new()
                        {
                            ItemCode = (string)recordsetUT.Fields.Item("ItemCode").Value,
                            ItemName = (string)recordsetUT.Fields.Item("ItemName").Value,
                            WhsCode = (string)recordsetUT.Fields.Item("WhsCode").Value,
                            WhsName = (string)recordsetUT.Fields.Item("WhsName").Value,
                            Disponible = (string)recordsetUT.Fields.Item("Disponible").Value
                        };
                        List_Inventario.Add(code);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_Inventario);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista con informacion del inventario de maquinaria" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de inventario de maquinaria: " + ex.Message });
            }
        }
        }
    }
