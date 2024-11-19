using CanellaMovilBackend.Models.SAPModels.Reports;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    public class SAPDataService
    {
        private string _connectionString;

        public SAPDataService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLSAP");
        }

        public async Task<List<CarteraConsolidada>> GetCarteraConsolidadaAsync(string storedProcedure, int empresa)
        {
            var cartera = new List<CarteraConsolidada>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new CarteraConsolidada
                            {
                                DocEntry = reader["DocEntry"].ToString(),
                                DocNum = reader["DocNum"].ToString(),
                                TransID = reader["TransID"].ToString(),
                                CodVendedor = reader["CodVendedor"].ToString(),
                                NomVendedor = reader["NomVendedor"].ToString(),
                                FechaFacturacion = reader["FechaFacturacion"].ToString(),
                                FechaVence = reader["FechaVence"].ToString(),
                                PagoNumero = reader["PagoNumero"].ToString(),
                                FacturaSerie = reader["FacturaSerie"].ToString(),
                                FacturaNumero = reader["FacturaNumero"].ToString(),
                                NumeroDocumento = reader["NumeroDocumento"].ToString(),
                                CentroCosto = reader["CentroCosto"].ToString(),
                                TipoDocumento = reader["TipoDocumento"].ToString(),
                                TipoPago = reader["TipoPago"].ToString(),
                                ClienteCodigo = reader["ClienteCodigo"].ToString(),
                                ClienteNombre = reader["ClienteNombre"].ToString(),
                                GrupoCliente = reader["GrupoCliente"].ToString(),
                                DireccionFISCAL = reader["DireccionFISCAL"].ToString(),
                                CobradorCodigo = reader["CobradorCodigo"].ToString(),
                                CobradorNombre = reader["CobradorNombre"].ToString(),
                                TotalFactura = reader["TotalFactura"].ToString(),
                                TotalCuota = reader["TotalCuota"].ToString(),
                                TotalPagado = reader["TotalPagado"].ToString(),
                                TotalPagadoxCuota = reader["TotalPagadoxCuota"].ToString(),
                                TotalSaldo = reader["TotalSaldo"].ToString(),
                                SALDO_0_30 = reader["SALDO_0_30"].ToString(),
                                SALDO_31_60 = reader["SALDO_31_60"].ToString(),
                                SALDO_61_90 = reader["SALDO_61_90"].ToString(),
                                SALDO_91_120 = reader["SALDO_91_120"].ToString(),
                                SALDO_120_ = reader["SALDO_120_"].ToString(),
                                AL_DIA = reader["AL_DIA"].ToString(),
                                MORA = reader["MORA"].ToString(),
                                Estado = reader["Estado"].ToString()
                            };
                            cartera.Add(item);
                        }
                    }
                }
            }

            return cartera;
        }
    }

}
