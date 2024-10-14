using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.AlertsManagement
{
    /// <summary>
    /// Clase para describir el documento enviado
    /// </summary>
    public class AlertDataLine
    {
        /// <summary>
        /// ObjectKey - DocEntry del documento en SAP
        /// </summary>
        [Required]
        public string ObjectKey { get; set; } = string.Empty;

        /// <summary>
        /// Object - El número de documento de SAP ejemplo 17 hace referencia a la orden de venta.
        /// </summary>
        [Required]
        public string Object { get; set; } = string.Empty;

        /// <summary>
        /// Value - Valor que se visualizara en la alerta DocNum haciendo referencia al DocEntry del documento.
        /// </summary>
        [Required]
        public string Value { get; set; } = string.Empty;
    }
}
