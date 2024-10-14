using SAPbobsCOM;

namespace CanellaMovilBackend.Service.SAPService
{
    /// <summary>
    /// Servicio que abre conexión con SAP B1
    /// </summary>
    public class SAPService : ISAPService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public SAPService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccesor = httpContextAccessor;
        }

        /// <summary>
        /// función que abre y retorna la conexión hacia SAP B1
        /// </summary>
        public Company SAPB1()
        {
            Company company;

            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appSettings.json")
           .Build();

            var compa = configuration.GetSection("SAPConnections").GetChildren().ToList().First();

            // Configurar los parámetros de conexión
            company = new Company();
            company.DbServerType = BoDataServerTypes.dst_MSSQL2014;
            company.Server = compa?.GetSection("Server")?.Value?.ToString() ?? "";
            var serv = compa?.GetSection("Company")?.Value?.ToString();
            company.CompanyDB = compa?.GetSection("CompanyDB")?.Value?.ToString();
            var compan = compa?.GetSection("Company")?.Value?.ToString();
            company.LicenseServer = compa?.GetSection("LicenseServer")?.Value?.ToString() ?? "";
            var lic = compa?.GetSection("LicenseServer")?.Value?.ToString() ?? "";
            company.DbUserName = compa?.GetSection("DbUserName")?.Value?.ToString() ?? "";
            var dbuse = compa?.GetSection("DbUserName")?.Value?.ToString() ?? "";
            company.DbPassword = compa?.GetSection("DbPassword")?.Value?.ToString() ?? "";
            var dbpass = compa?.GetSection("DbPassword")?.Value?.ToString() ?? "";
            company.UserName = compa?.GetSection("UserName")?.Value?.ToString() ?? "";
            var usname = compa?.GetSection("UserName")?.Value?.ToString() ?? "";
            company.Password = compa?.GetSection("Password")?.Value?.ToString() ?? "";
            var campnn = compa?.GetSection("Password")?.Value?.ToString() ?? "";
            company.UseTrusted = false;
            company.language = BoSuppLangs.ln_English;
            company.Connect();

            return company;
        }

        /// <summary>
        /// Desconecta de SAP
        /// </summary>
        /// <param name="company"></param>
        public void SAPB1_DISCONNECT(Company company)
        {
            if (company != null && company.Connected)
            {
                company.Disconnect();
            }
        }
    }
}
