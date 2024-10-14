using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.AlertsManagement
{
    /// <summary>
    /// Clase para listar los documentos que se enviaran en la alerta
    /// </summary>
    public class AlertDataColumn
    {
        /// <summary>
        /// ColumnName - Nombre de la columna enviada.
        /// </summary>
        [Required]
        public string ColumnName { get; set; } = string.Empty;

        /// <summary>
        /// Lines - Lista todas las filas enviadas en la alerta de SAP
        /// </summary>
        [Required]
        public List<AlertDataLine> Lines { get; set; } = [];
    }
}
