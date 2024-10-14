using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.AlertsManagement
{
    /// <summary>
    /// Clase para el envió de las alertas SAP.
    /// </summary>
    public class AlertSAP
    {
        /// <summary>
        /// Subject - Asunto de la alerta
        /// </summary>
        [Required]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Text - Cuerpo de la alerta.
        /// </summary>
        [Required]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// AlertUsers - Listado de los usuario SAP que se enviaran la alerta
        /// </summary>
        [Required]
        public List<AlertUser> AlertUsers { get; set; } = [];

        /// <summary>
        /// AlertDataColumns - Lista todos los documentos que se quieren enviar en la alerta
        /// </summary>
        public List<AlertDataColumn> AlertDataColumns { get; set; } = [];
    }
}
