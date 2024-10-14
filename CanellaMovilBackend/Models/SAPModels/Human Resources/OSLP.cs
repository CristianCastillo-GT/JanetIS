namespace CanellaMovilBackend.Models.SAPModels.Human_Resources
{
    /// <summary>
    ///  Modelos de la tabla OSLP SAP 
    /// </summary>
    public class OSLP
    {
        /// <summary>
        /// SlpCode - Codigo de Vendedor
        /// </summary>
        public string SlpCode { get; set; } = string.Empty;

        /// <summary>
        /// SlpName - Sales Employe Name
        /// </summary>
        public string SlpName { get; set; } = string.Empty;

        /// <summary>
        /// Correo Electrónico
        /// </summary>
        public string U_Email { get; set; } = string.Empty;
    }
}
