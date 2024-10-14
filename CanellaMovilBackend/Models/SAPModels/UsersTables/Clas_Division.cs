using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.UsersTables
{
    /// <summary>
    /// Modelo de tabla de usuario de SAP [@Clas_Division]
    /// </summary>
    public class Clas_Division
    {
        /// <summary>
        /// Code - Codigo de Division
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Name / Nombre de Division
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
