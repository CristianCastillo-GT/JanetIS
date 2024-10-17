using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.UsersTables;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using static CanellaMovilBackend.Models.SAPModels.UsersTables.URequestData;

namespace CanellaMovilBackend.Controllers.UserTableControllers
{
    /// <summary>
    /// Controlador de categoria
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    public class UserTablesController : Controller
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        /// 
        public UserTablesController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Obtiene los datos de categoria
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<Clas_Categoria>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetCategoria(RequestGetCategoria request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.CodeDivision ??= "";
                request.CodeTipo ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WCAN_SELECT_CLAS_CATEGORIA '"+ request.CodeDivision+"','"+ request.CodeTipo+"'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<Clas_Categoria> List_Clas_Categoria = [];
                    while (!recordsetUT.EoF)
                    {
                        Clas_Categoria categoria = new()
                        {
                            Code = (string)recordsetUT.Fields.Item("Code").Value,
                            Name = (string)recordsetUT.Fields.Item("Name").Value
                        };

                        List_Clas_Categoria.Add(categoria);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_Clas_Categoria);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista de Categorias" });
                }

            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de categorias: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los datos de division
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<Clas_Division>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetDivision( RequestGetDivision request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;
                request.CodeCategoria ??= "";
                request.CodeTipo ??= "";
                // Obtener el objeto division de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WCAN_SELECT_CLAS_DIVISION '"+ request.CodeCategoria+"', '"+ request.CodeTipo+"'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<Clas_Division> List_Clas_Division = [];
                    while (!recordsetUT.EoF)
                    {
                        Clas_Division division = new()
                        {
                            Code = (string)recordsetUT.Fields.Item("Code").Value,
                            Name = (string)recordsetUT.Fields.Item("Name").Value
                        };

                        List_Clas_Division.Add(division);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_Clas_Division);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista de Divisiones" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de divisiones: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los datos de tipo
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<Clas_Division>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetTipo(RequestGetTipo request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.CodeCategoria ??= "";
                request.CodeDivision ??= "";

                // Obtener el objeto division de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WCAN_SELECT_CLAS_TIPO '"+ request.CodeCategoria+"', '"+ request.CodeDivision+"'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<Clas_Tipo> List_Clas_Tipo = [];
                    while (!recordsetUT.EoF)
                    {
                        Clas_Tipo tipo = new()
                        {
                            Code = (string)recordsetUT.Fields.Item("Code").Value,
                            Name = (string)recordsetUT.Fields.Item("Name").Value
                        };

                        List_Clas_Tipo.Add(tipo);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_Clas_Tipo);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista de Divisiones" });
                }

            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de divisiones: " + ex.Message });
            }
        }
    }
}
