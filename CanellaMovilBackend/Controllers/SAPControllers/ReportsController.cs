using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.Reports;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using System.Data.SqlClient;
using System.Globalization;
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
    [ServiceFilter(typeof(SAPConnectionFilter))]
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

            try

            {

                string connectionString;

                string storedProcedure, statusQuery, resultQuery;

                string processId = Guid.NewGuid().ToString();

                // Configuración de conexión y procedimientos

                var configuration = new ConfigurationBuilder()

                    .SetBasePath(Directory.GetCurrentDirectory())

                    .AddJsonFile("appSettings.json")

                    .Build();

                switch (empresa)

                {

                    case 1:

                        connectionString = configuration.GetConnectionString("SQLSAP");

                        storedProcedure = "[CRCO_STOD_CarteraCanella_ProcesarEstado_Odoo]";

                        statusQuery = "SELECT Status FROM [UTILS].[dbo].CRCO_ControlTable WHERE ProcessID = @ProcessID ORDER BY [StartTime] DESC";

                        resultQuery = "SELECT * FROM [UTILS].[dbo].CRCO_ResultadoCartera WHERE ProcessID = @ProcessID";

                        break;

                    case 2:

                        connectionString = configuration.GetConnectionString("VESA");

                        storedProcedure = "[SBO_VESA].[dbo].[CRCO_STOD_CarteraVESA_ProcesarEstado_Odoo]";

                        statusQuery = "SELECT Status FROM [UTILS_VESA].[dbo].CRCO_ControlTable WHERE ProcessID = @ProcessID ORDER BY [StartTime] DESC";

                        resultQuery = "SELECT * FROM [UTILS_VESA].[dbo].CRCO_ResultadoCartera WHERE ProcessID = @ProcessID";

                        break;

                    case 3:

                        connectionString = configuration.GetConnectionString("TALLER");

                        storedProcedure = "[TALLER].[dbo].[CRCO_STOD_CarteraMAUTO_ProcesarEstado_Odoo]";

                        statusQuery = "SELECT Status FROM [UTILS].[dbo].CRCO_ControlTable WHERE ProcessID = @ProcessID ORDER BY [StartTime] DESC";

                        resultQuery = "SELECT * FROM [UTILS].[dbo].CRCO_ResultadoCartera WHERE ProcessID = @ProcessID";

                        break;

                    case 4:

                        connectionString = configuration.GetConnectionString("MAQUIPOS");

                        storedProcedure = "[SBO_MAQUIPOS].[dbo].[CRCO_STOD_CarteraMAQUIPOS_ProcesarEstado_Odoo]";

                        statusQuery = "  SELECT Status FROM [UTILS_MAQUIPOS].[dbo].CRCO_ControlTable WHERE ProcessID = @ProcessID ORDER BY [StartTime] DESC";

                        resultQuery = "SELECT * FROM [UTILS_MAQUIPOS].[dbo].CRCO_ResultadoCartera WHERE ProcessID = @ProcessID";

                        break;

                    default:

                        return BadRequest(new MessageAPI() { Result = "Fail", Message = "Empresa no válida", CodeNum = "(020-010)" });

                }

                using (SqlConnection connection = new SqlConnection(connectionString))

                {

                    connection.Open();

                    // Ejecutar procedimiento almacenado para iniciar el proceso

                    using (SqlCommand command = new SqlCommand($"EXEC {storedProcedure} @ProcessID", connection))

                    {

                        command.CommandTimeout = 600; // 10 minutos

                        command.Parameters.AddWithValue("@ProcessID", processId);

                        command.ExecuteNonQuery();

                    }

                    // Verificar el estado del proceso periódicamente

                    bool isCompleted = false;

                    while (!isCompleted)

                    {

                        using (SqlCommand statusCommand = new SqlCommand(statusQuery, connection))

                        {

                            statusCommand.Parameters.AddWithValue("@ProcessID", processId);

                            string status = statusCommand.ExecuteScalar()?.ToString();

                            if (status == "Completed")

                            {

                                isCompleted = true;

                            }

                            else

                            {

                                System.Threading.Thread.Sleep(5000); // Espera 5 segundos

                            }

                        }

                    }

                    // Recuperar los resultados del procedimiento almacenado

                    using (SqlCommand resultCommand = new SqlCommand(resultQuery, connection))

                    {

                        resultCommand.Parameters.AddWithValue("@ProcessID", processId);

                        using (SqlDataReader reader = resultCommand.ExecuteReader())

                        {

                            if (reader.HasRows)

                            {

                                List<CarteraConsolidada> ListCarteraConsolidada = new();

                                while (reader.Read())

                                {

                                    CarteraConsolidada carteraConsolidada = new()

                                    {

                                        DocEntry = reader["DocEntry"]?.ToString(),

                                        DocNum = reader["DocNum"]?.ToString(),

                                        TransID = reader["TransID"]?.ToString(),

                                        CodVendedor = reader["CodVendedor"]?.ToString(),

                                        NomVendedor = reader["NomVendedor"]?.ToString(),

                                        FechaFacturacion = reader["FechaFacturacion"]?.ToString(),

                                        FechaVence = reader["FechaVence"]?.ToString(),

                                        PagoNumero = reader["PagoNumero"]?.ToString(),

                                        FacturaSerie = reader["FacturaSerie"]?.ToString(),

                                        FacturaNumero = reader["FacturaNumero"]?.ToString(),

                                        NumeroDocumento = reader["NumeroDocumento"]?.ToString(),

                                        CentroCosto = reader["CentroCosto"]?.ToString(),

                                        TipoDocumento = reader["TipoDocumento"]?.ToString(),

                                        TipoPago = reader["TipoPago"]?.ToString(),

                                        ClienteCodigo = reader["ClienteCodigo"]?.ToString(),

                                        ClienteNombre = reader["ClienteNombre"]?.ToString(),

                                        GrupoCliente = reader["GrupoCliente"]?.ToString(),

                                        DireccionFISCAL = reader["DireccionFISCAL"]?.ToString(),

                                        CobradorCodigo = reader["CobradorCodigo"]?.ToString(),

                                        CobradorNombre = reader["CobradorNombre"]?.ToString(),

                                        TotalFactura = reader["TotalFactura"]?.ToString(),

                                        TotalCuota = reader["TotalCuota"]?.ToString(),

                                        TotalPagado = reader["TotalPagado"]?.ToString(),

                                        TotalPagadoxCuota = reader["TotalPagadoxCuota"]?.ToString(),

                                        TotalSaldo = reader["TotalSaldo"]?.ToString(),

                                        SALDO_0_30 = reader["SALDO_0_30"]?.ToString(),

                                        SALDO_31_60 = reader["SALDO_31_60"]?.ToString(),

                                        SALDO_61_90 = reader["SALDO_61_90"]?.ToString(),

                                        SALDO_91_120 = reader["SALDO_91_120"]?.ToString(),

                                        SALDO_120_ = reader["SALDO_120_"]?.ToString(),

                                        AL_DIA = reader["AL_DIA"]?.ToString(),

                                        MORA = reader["MORA"]?.ToString(),

                                        Estado = reader["Estado"]?.ToString()

                                    };

                                    ListCarteraConsolidada.Add(carteraConsolidada);

                                }

                                return Ok(new MessageAPI()
                                {
                                    Result = "OK",
                                    Message = "SQL procesó la cartera y debe de llevar al menos un registro",
                                    CodeNum = "(900)",
                                    Data = ListCarteraConsolidada
                                });

                            }

                            else

                            {

                                return Ok(new MessageAPI() { Result = "OK", Message = "(Cartera Liquidada o al dia) No se encontró ningún registro", CodeNum = "(010)" });

                            }

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de la cartera - " + ex.Message, CodeNum = "(020-020)" });

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
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

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
                    return Ok(ListPagosClientes);
                }
                else
                {
                    return Ok(new MessageAPI() { Result = "OK", Message = "No se encontró ningun registro" });
                }
            }
            catch (Exception ex)
            {
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
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                string storedProcedure2;

                var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appSettings.json")
              .Build();

                switch (empresa)
                {
                    case 1:
                        companyConnection = sapService.SAPB1();
                        storedProcedure2 = ("[UTILS].[dbo].[CRCO_ProcesarPromesa] '" + request.CardCode + "', '" + request.fechaPromesa + "', '" + request.amount + "', '" + request.status + "', '" + request.fechaCreacion + "', '" + request.idPromesa + "', '" + request.docentry + "', '" + request.porDocumento + "'");
                        break;
                    case 2:
                        companyConnection = sapService.SAPB1();
                        storedProcedure2 = ($"[{configuration.GetConnectionString("VESA") ?? ""}].[UTILS_VESA].[dbo].[CRCO_procesarPromesa] '" + request.CardCode + "', '" + request.fechaPromesa + "', '" + request.amount + "', '" + request.status + "', '" + request.fechaCreacion + "', '" + request.idPromesa + "', '" + request.docentry + "', '" + request.porDocumento + "'");
                        break;
                    case 3:
                        companyConnection = sapService.SAPB1();
                        storedProcedure2 = ($"[{configuration.GetConnectionString("TALLER") ?? ""}].[[UTILS].[dbo].[CRCO_procesarPromesa] '" + request.CardCode + "', '" + request.fechaPromesa + "', '" + request.amount + "', '" + request.status + "', '" + request.fechaCreacion + "', '" + request.idPromesa + "', '" + request.docentry + "', '" + request.porDocumento + "'");
                        break;
                    case 4:
                        companyConnection = sapService.SAPB1();
                        storedProcedure2 = ($"[{configuration.GetConnectionString("MAQUIPOS") ?? ""}].[UTILS_MAQUIPOS].[dbo].[CRCO_procesarPromesa] '" + request.CardCode + "', '" + request.fechaPromesa + "', '" + request.amount + "', '" + request.status + "', '" + request.fechaCreacion + "', '" + request.idPromesa + "', '" + request.docentry + "', '" + request.porDocumento + "'");
                        break;
                    default:
                        return BadRequest(new MessageAPI() { Result = "Fail", Message = "Empresa no válida" });
                }

                Company company = companyConnection.Company;

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
                    return Ok(ListaEstadoPromesa);
                }
                else
                {
                    return Ok(new MessageAPI() { Result = "OK", Message = "No se encontró ninguna Promesa asociada" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de promesas  - " + ex.Message });
            }
        }
    }
}
