using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.SAPModels.Reports;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using static CanellaMovilBackend.Models.SAPModels.Reports.CCRequestData;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de reportes de SAP
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class ReportsController(ISAPService sapService) : ControllerBase
    {
        /// <summary>
        /// Obtiene el listado de la cartera consolidada
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CarteraConsolidada>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetCarteraConsolidada(int empresa)
        {
            Company company = sapService.SAPB1();
            try
            {
                //CompanyConnection companyConnection;
                string storedProcedure;
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();
                // Selecciona el servidor y el procedimiento almacenado en base al parámetro empresa
                switch (empresa)
                {
                    case 1:
                        storedProcedure = "[CRCO_STOD_CarteraCanella_ProcesarEstado_Odoo]";
                        break;
                    case 2:
                        storedProcedure = $"[{configuration.GetConnectionString("VESA") ?? ""}].[SBO_VESA].[dbo].[CRCO_STOD_CarteraVESA_ProcesarEstado_Odoo]";
                        break;
                    case 3:
                        storedProcedure = $"[{configuration.GetConnectionString("TALLER") ?? ""}].[TALLER].[dbo].[CRCO_STOD_CarteraMAUTO_ProcesarEstado_Odoo]";
                        break;
                    case 4:
                        storedProcedure = $"[{configuration.GetConnectionString("MAQUIPOS") ?? ""}].[SBO_MAQUIPOS].[dbo].[CRCO_STOD_CarteraMAQUIPOS_ProcesarEstado_Odoo]";
                        break;
                    default:
                        return BadRequest(new MessageAPI() { Result = "Fail", Message = "Empresa no válida" });
                }
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery($"EXEC {storedProcedure}");

                if (recordset.RecordCount > 0)
                {
                    List<CarteraConsolidada> ListCarteraConsolidada = [];
                    while (!recordset.EoF)
                    {
                        CarteraConsolidada? carteraConsolidada = new()
                        {
                            DocEntry = Convert.ToString(recordset.Fields.Item("DocEntry").Value),
                            DocNum = Convert.ToString(recordset.Fields.Item("DocNum").Value),
                            TransID = Convert.ToString(recordset.Fields.Item("TransID").Value),
                            CodVendedor = Convert.ToString(recordset.Fields.Item("CodVendedor").Value),
                            NomVendedor = Convert.ToString(recordset.Fields.Item("NomVendedor").Value),
                            FechaFacturacion = Convert.ToString(recordset.Fields.Item("FechaFacturacion").Value),
                            FechaVence = Convert.ToString(recordset.Fields.Item("FechaVence").Value),
                            PagoNumero = Convert.ToString(recordset.Fields.Item("PagoNumero").Value),
                            FacturaSerie = Convert.ToString(recordset.Fields.Item("FacturaSerie").Value),
                            FacturaNumero = Convert.ToString(recordset.Fields.Item("FacturaNumero").Value),
                            NumeroDocumento = Convert.ToString(recordset.Fields.Item("NumeroDocumento").Value),
                            CentroCosto = Convert.ToString(recordset.Fields.Item("CentroCosto").Value),
                            TipoDocumento = Convert.ToString(recordset.Fields.Item("TipoDocumento").Value),
                            TipoPago = Convert.ToString(recordset.Fields.Item("TipoPago").Value),
                            ClienteCodigo = Convert.ToString(recordset.Fields.Item("ClienteCodigo").Value),
                            ClienteNombre = Convert.ToString(recordset.Fields.Item("ClienteNombre").Value),
                            GrupoCliente = Convert.ToString(recordset.Fields.Item("GrupoCliente").Value),
                            DireccionFISCAL = Convert.ToString(recordset.Fields.Item("DireccionFISCAL").Value),
                            CobradorCodigo = Convert.ToString(recordset.Fields.Item("CobradorCodigo").Value),
                            CobradorNombre = Convert.ToString(recordset.Fields.Item("CobradorNombre").Value),
                            TotalFactura = Convert.ToString(recordset.Fields.Item("TotalFactura").Value),
                            TotalCuota = Convert.ToString(recordset.Fields.Item("TotalCuota").Value),
                            TotalPagado = Convert.ToString(recordset.Fields.Item("TotalPagado").Value),
                            TotalPagadoxCuota = Convert.ToString(recordset.Fields.Item("TotalPagadoxCuota").Value),
                            TotalSaldo = Convert.ToString(recordset.Fields.Item("TotalSaldo").Value),
                            SALDO_0_30 = Convert.ToString(recordset.Fields.Item("SALDO_0_30").Value),
                            SALDO_31_60 = Convert.ToString(recordset.Fields.Item("SALDO_31_60").Value),
                            SALDO_61_90 = Convert.ToString(recordset.Fields.Item("SALDO_61_90").Value),
                            SALDO_91_120 = Convert.ToString(recordset.Fields.Item("SALDO_91_120").Value),
                            SALDO_120_ = Convert.ToString(recordset.Fields.Item("SALDO_120_").Value),
                            AL_DIA = Convert.ToString(recordset.Fields.Item("AL_DIA").Value),
                            MORA = Convert.ToString(recordset.Fields.Item("MORA").Value),
                            Estado = Convert.ToString(recordset.Fields.Item("Estado").Value)
                        };

                        ListCarteraConsolidada.Add(carteraConsolidada);
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(ListCarteraConsolidada);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(new MessageAPI() { Result = "OK", Message = "No se encontró ningun registro" });
                }

            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de la cartera - " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene el listado de pagos realizados por clientes
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<PagosCreditos>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetPagosPorClientes()
        {
            Company company = sapService.SAPB1();
            try
            {
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery("EXEC [dbo].[STOD_CarteraCanella_ConPlanesPagoResumen]");
                if (recordset.RecordCount > 0)
                {
                    List<PagosCreditos> ListPagosClientes = [];
                    while (!recordset.EoF)
                    {
                        PagosCreditos pagosCreditos = new()
                        {
                            DocEntry = Convert.ToString(recordset.Fields.Item("DocEntry").Value),
                            DocNum = Convert.ToString(recordset.Fields.Item("DocNum").Value),
                            TransID = Convert.ToString(recordset.Fields.Item("TransID").Value),
                            FechaFacturacion = Convert.ToString(recordset.Fields.Item("FechaFacturacion").Value),
                            FacturaSerie = Convert.ToString(recordset.Fields.Item("FacturaSerie").Value),
                            FacturaNumero = Convert.ToString(recordset.Fields.Item("FacturaNumero").Value),
                            CentroCosto = Convert.ToString(recordset.Fields.Item("CentroCosto").Value),
                            TipoDocumento = Convert.ToString(recordset.Fields.Item("TipoDocumento").Value),
                            TipoPago = Convert.ToString(recordset.Fields.Item("TipoPago").Value),
                            ClienteCodigo = Convert.ToString(recordset.Fields.Item("ClienteCodigo").Value),
                            ClienteNombre = Convert.ToString(recordset.Fields.Item("ClienteNombre").Value),
                            GrupoCliente = Convert.ToString(recordset.Fields.Item("GrupoCliente").Value),
                            DireccionFISCAL = Convert.ToString(recordset.Fields.Item("DireccionFISCAL").Value),
                            CobradorCodigo = Convert.ToString(recordset.Fields.Item("CobradorCodigo").Value),
                            CobradorNombre = Convert.ToString(recordset.Fields.Item("CobradorNombre").Value),
                            TotalFactura = Convert.ToString(recordset.Fields.Item("TotalFactura").Value),
                            TotalPagado = Convert.ToString(recordset.Fields.Item("TotalPagado").Value),
                            TotalSaldo = Convert.ToString(recordset.Fields.Item("TotalSaldo").Value),
                            SALDO_0_30 = Convert.ToString(recordset.Fields.Item("SALDO_0_30").Value),
                            SALDO_31_60 = Convert.ToString(recordset.Fields.Item("SALDO_31_60").Value),
                            SALDO_61_90 = Convert.ToString(recordset.Fields.Item("SALDO_61_90").Value),
                            SALDO_91_120 = Convert.ToString(recordset.Fields.Item("SALDO_91_120").Value),
                            SALDO_120_ = Convert.ToString(recordset.Fields.Item("SALDO_120_").Value)

                        };
                        ListPagosClientes.Add(pagosCreditos);
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(ListPagosClientes);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(new MessageAPI() { Result = "OK", Message = "No se encontró ningun registro" });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de pagos por cliente - " + ex.Message });
            }
        }
        /// <summary>
        /// Obtiene la actualización de la promesa de pago 
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<EstadoPromesa>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetEstadoPromesa(RequestPayPromise request, int empresa)
        {
            Company company = sapService.SAPB1();
            try
            {
                string storedProcedure2;

                var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appSettings.json")
              .Build();

                switch (empresa)
                {
                    case 1:
                        storedProcedure2 = ("[UTILS].[dbo].[CRCO_ProcesarPromesa] '" + request.CardCode + "', '" + request.fechaPromesa + "', '" + request.amount + "', '" + request.status + "', '" + request.fechaCreacion + "', '" + request.idPromesa + "', '" + request.docentry + "', '" + request.porDocumento + "'");
                        break;
                    case 2:
                        storedProcedure2 = ($"[{configuration.GetConnectionString("VESA") ?? ""}].[UTILS_VESA].[dbo].[CRCO_procesarPromesa] '" + request.CardCode + "', '" + request.fechaPromesa + "', '" + request.amount + "', '" + request.status + "', '" + request.fechaCreacion + "', '" + request.idPromesa + "', '" + request.docentry + "', '" + request.porDocumento + "'");
                        break;
                    case 3:
                        storedProcedure2 = ($"[{configuration.GetConnectionString("TALLER") ?? ""}].[[UTILS].[dbo].[CRCO_procesarPromesa] '" + request.CardCode + "', '" + request.fechaPromesa + "', '" + request.amount + "', '" + request.status + "', '" + request.fechaCreacion + "', '" + request.idPromesa + "', '" + request.docentry + "', '" + request.porDocumento + "'");
                        break;
                    case 4:
                        storedProcedure2 = ($"[{configuration.GetConnectionString("MAQUIPOS") ?? ""}].[UTILS_MAQUIPOS].[dbo].[CRCO_procesarPromesa] '" + request.CardCode + "', '" + request.fechaPromesa + "', '" + request.amount + "', '" + request.status + "', '" + request.fechaCreacion + "', '" + request.idPromesa + "', '" + request.docentry + "', '" + request.porDocumento + "'");
                        break;
                    default:
                        return BadRequest(new MessageAPI() { Result = "Fail", Message = "Empresa no válida" });
                }

                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery($"EXEC {storedProcedure2}");

                if (recordset.RecordCount > 0)
                {
                    List<EstadoPromesa> ListaEstadoPromesa = [];
                    while (!recordset.EoF)
                    {
                        EstadoPromesa estadoPromesa = new()
                        {
                            resp_status = Convert.ToString(recordset.Fields.Item("resp_status").Value),
                            resp_message = Convert.ToString(recordset.Fields.Item("resp_message").Value)
                        };
                        ListaEstadoPromesa.Add(estadoPromesa);
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(ListaEstadoPromesa);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(new MessageAPI() { Result = "OK", Message = "No se encontró ninguna Promesa asociada" });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de promesas  - " + ex.Message });
            }
        }
    }
}
