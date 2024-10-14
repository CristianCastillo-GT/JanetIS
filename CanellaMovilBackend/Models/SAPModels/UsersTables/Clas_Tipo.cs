using System.ComponentModel.DataAnnotations;


namespace CanellaMovilBackend.Models.SAPModels.UsersTables
{
    /// <summary>
    /// Modelo de tabla de usuario de SAP [@Clas_Tipo]
    /// </summary>
    public class Clas_Tipo
    {
        /// <summary>
        /// Code - Codigo de Tipo
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Name / Nombre de Tipo
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
