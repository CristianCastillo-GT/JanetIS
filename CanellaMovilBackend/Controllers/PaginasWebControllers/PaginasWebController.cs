using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.PageCanon;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using static CanellaMovilBackend.Models.PaginasWebModels.MRequestData;
using CanellaMovilBackend.Models.PaginasWebModels;
using CanellaMovilBackend.Filters.UserFilter;

namespace CanellaMovilBackend.Controllers.PaginasWebControllers
{
    /// <summary>
    /// Controlador de paginas web
    /// </summary>
    ///[Authorize]
    ///[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    //[ServiceFilter(typeof(RoleFilter))]
    //[ServiceFilter(typeof(SAPConnectionFilter))]
    //[ServiceFilter(typeof(ResultAllFilter))]
    public class PaginasWebController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        /// 
        public PaginasWebController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Obtiene los datos de inventario de maquinaria de sap canella y maquipos
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<InventarioMaquinaria>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetInventarioMaquinaria(MRequestData.RequestGetInventarioMaquinaria request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.AddId ??= "";
                request.clsEmpresa ??= "";
                request.PlataformaConsumo ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WSPW_SELECT_INVENTORY_MAQUINARIA '" + request.AddId + "', '" + request.clsEmpresa + "', '" + request.PlataformaConsumo + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<InventarioMaquinaria> List_Inventario = [];
                    while (!recordsetUT.EoF)
                    {
                        InventarioMaquinaria? code = new()
                        {
                            Empresa = (string)recordsetUT.Fields.Item("Empresa").Value,
                            ItemCode = (string)recordsetUT.Fields.Item("ItemCode").Value,
                            ItemName = (string)recordsetUT.Fields.Item("ItemName").Value,
                            Price = (string)recordsetUT.Fields.Item("Price").Value
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

        /// <summary>
        /// Obtiene los datos de inventario de maquinaria de sap canella y maquipos
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<ClientesFacturacion>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetClienteFacturacion(MRequestData.RequestGetClientesFacturacionMaquinaria request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.AddId ??= "";
                request.clsEmpresa ??= "";
               
                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WSPW_SELECT_FACTURACION_MAQUINARIA '" + request.AddId + "', '" + request.clsEmpresa + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<ClientesFacturacion> List_Clientes = [];
                    while (!recordsetUT.EoF)
                    {
                        ClientesFacturacion? code = new()
                        {
                            Empresa = (string)recordsetUT.Fields.Item("Empresa").Value,
                            CardCode = (string)recordsetUT.Fields.Item("CardCode").Value,
                            CardName = (string)recordsetUT.Fields.Item("CardName").Value,
                            CardFName = (string)recordsetUT.Fields.Item("CardFName").Value,
                            addid = (string)recordsetUT.Fields.Item("addid").Value,
                            phone1 = (string)recordsetUT.Fields.Item("phone1").Value,
                            e_mail = (string)recordsetUT.Fields.Item("e_mail").Value,
                            street = (string)recordsetUT.Fields.Item("street").Value,
                            county = (string)recordsetUT.Fields.Item("county").Value,
                            country = (string)recordsetUT.Fields.Item("country").Value,
                            Facturacion = (string)recordsetUT.Fields.Item("Facturacion").Value
                        };
                        List_Clientes.Add(code);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_Clientes);
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
