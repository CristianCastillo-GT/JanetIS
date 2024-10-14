using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.AlertsManagement
{
    /// <summary>
    /// Clase para listar a los usuario que se enviaran las alertas
    /// </summary>
    public class AlertUser
    {
        /// <summary>
        /// UserCode - Código de usuario en SAP.
        /// </summary>
        [Required]
        public string UserCode { get; set; } = string.Empty;
    }
}
