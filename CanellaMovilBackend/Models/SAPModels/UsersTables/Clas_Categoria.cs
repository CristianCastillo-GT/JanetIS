using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.UsersTables
{

    /// <summary>
    /// Modelo de tabla de usuario de SAP [@Clas_Categoria]
    /// </summary>
    public class Clas_Categoria
    {
        /// <summary>
        /// Code - Codigo de Categoria
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Name / Nombre de Categoria
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

    }
}
