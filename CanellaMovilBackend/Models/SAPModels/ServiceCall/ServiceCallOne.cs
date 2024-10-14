using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.ServiceCall
{
    /// <summary>
    /// Obtiene una llamada de servicio
    /// </summary>
    public class ServiceCallOne
    {
        /// <summary>
        /// Identificador unico de la llamada de servicio
        /// </summary>
        [Required]
        public int callID { get; set; }

        /// <summary>
        /// Fecha de creación
        /// </summary>
        [Required]
        public string createDate { get; set; } = string.Empty;

        /// <summary>
        /// Estado de la llamada de servicio
        /// </summary>
        [Required]
        public string status { get; set; } = string.Empty;
        
        /// <summary>
        /// Centro de Costo de la llamada de servicio
        /// </summary>
        public string? U_CECO { get; set; }
    }
}
