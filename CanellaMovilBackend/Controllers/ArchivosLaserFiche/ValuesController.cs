using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.PageCanon;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using SAPbobsCOM;
using static CanellaMovilBackend.Models.ArchivosLaserFiche.LaserFicheRequestData;
using CanellaMovilBackend.Models.ArchivosLaserFiche;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models.PaginasWebModels;
using CanellaMovilBackend.Models.MotulTopKe;

namespace CanellaMovilBackend.Controllers.ArchivosLaserFiche
{
    /// <summary>
    /// Controlador de Laserfiche
    /// </summary>
    ///[Authorize]
    ///[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    //[ServiceFilter(typeof(RoleFilter))]
    //[ServiceFilter(typeof(SAPConnectionFilter))]
    //[ServiceFilter(typeof(ResultAllFilter))]
    public class ValuesController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        /// 
        public ValuesController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Traslada archivos de un destino a otro
        /// </summary>
        /// <response code="200">Traslado de archivos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult MoveFiles(LaserFicheRequestData.RequestGetDirectoriosLaserFiche request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.clsEmpresa ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WSLaserFiche_SELECT_Archivos '" + request.clsEmpresa + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<DataArchivos> List_Inventario = [];
                    while (!recordsetUT.EoF)
                    {
                        DataArchivos? code = new()
                        {
                            DocNum = (string)recordsetUT.Fields.Item("DocNum").Value,
                            ObjType = (string)recordsetUT.Fields.Item("ObjType").Value,
                            FileName = (string)recordsetUT.Fields.Item("FileName").Value,
                            INPath = (string)recordsetUT.Fields.Item("INPath").Value,
                            OUTPath = (string)recordsetUT.Fields.Item("OUTPath").Value
                        };
                        List_Inventario.Add(code);
                        recordsetUT.MoveNext();
                    }

                    foreach (var item in List_Inventario)
                    {
                        if (System.IO.File.Exists(item.INPath))
                        {
                            // Copia el archivo a la nueva ubicación
                            System.IO.File.Copy(item.INPath, item.OUTPath, true); // El tercer parámetro 'true' sobrescribe si el archivo ya existe

                        }

                        if (System.IO.File.Exists(item.OUTPath))
                        {
                            Recordset recordsetUT2 = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                            recordsetUT2.DoQuery("EXEC WSLaserFiche_UPDATE_EstadoArchivos " + item.DocNum + "," + item.ObjType + "");
                        }


                    }
                    return Ok("Archivos trasladados correctamente.");
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "El archivo de origen no existe." });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "Error al trasladar el archivo: " + ex.Message });

            }
        }

            private static string ModifyPath(string path)
        {
            // Reemplaza las barras invertidas dobles con simples, dejando solo la primera como doble
            if (path.StartsWith("\\"))
            {
                return "\\" + path.Substring(1).Replace("\\", "\\");
            }
            return path.Replace("\\", "\\");
        }

    }
}
