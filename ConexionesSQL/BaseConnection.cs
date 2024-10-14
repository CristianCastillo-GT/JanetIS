using Microsoft.Extensions.Configuration;

namespace ConexionesSQL
{
    public class BaseConnection
    {
        protected string connectionSTOD;
        protected string connectionSISCON;
        protected string connectionSAP;
        protected string connectionUTILS;

        public BaseConnection()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();
            // Obtener las conexiones
            this.connectionSTOD = configuration.GetConnectionString("SQLSTOD") ?? "";
            this.connectionSISCON = configuration.GetConnectionString("SQLSISCON") ?? "";
            this.connectionSAP = configuration.GetConnectionString("SQLSAP") ?? "";
            this.connectionUTILS = configuration.GetConnectionString("SQLUTILS") ?? "";
        }
    }
}
