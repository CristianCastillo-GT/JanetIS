using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.PowerAutomateModels
{
    /// <summary>
    /// Clase que indica la respuesta de la boleta
    /// </summary>
    public class MessageBoleta
    {
        /// <summary>
        /// status - Estado que indica que se actualizo correctamente
        /// </summary>
        [Required]
        public string status { get; set; } = string.Empty;
    }
}
