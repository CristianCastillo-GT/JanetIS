namespace CanellaMovilBackend.Models.SAPModels
{
    /// <summary>
    /// Objeto para obtener la configuración de la base de datos SAP
    /// </summary>
    public class ConnectionConfig
    {

        /// <summary>
        /// Company / Nombre de la Empresa
        /// </summary>
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// Server / Servidor
        /// </summary>
        public string Server { get; set; } = string.Empty;

        /// <summary>
        /// CompanyDB / Nombre de la base de datos
        /// </summary>
        public string CompanyDB { get; set; } = string.Empty;

        /// <summary>
        /// LicenseServer / IP del servidor de licencias
        /// </summary>
        public string LicenseServer { get; set; } = string.Empty;

        /// <summary>
        /// DbUserName / Usuario de la base de datos
        /// </summary>
        public string DbUserName { get; set; } = string.Empty;

        /// <summary>
        /// DbPassword / Contraseña para el usuario de la base de datos
        /// </summary>
        public string DbPassword { get; set; } = string.Empty;

        /// <summary>
        /// UserName / Usuario de SAP
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Password / Contraseña para el usuario de SAP
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
