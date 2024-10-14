using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels
{
    /// <summary>
    /// Modelo global del cierre de entregas
    /// </summary>
    public class ListOpenDelivery
    {
        /// <summary>
        /// Comentario del cierre de la entrega
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Listado de entregas abiertas
        /// </summary>
        public List<OpenDelivery> ListDelivery { get; set; } = [];
    }

    /// <summary>
    /// Modelo para el cierre de entregas
    /// </summary>
    public class OpenDelivery
    {
        /// <summary>
        /// Identificador del documento
        /// </summary>
        [Required]
        public int DocNum { get; set; }

        /// <summary>
        /// Identificador unico
        /// </summary>
        [Required]
        public int DocEntry { get; set; } = 0;

        /// <summary>
        /// Estado de la entrega
        /// </summary>
        [Required]
        [DefaultValue(1)]
        public StateDelivery State { get; set; }

        /// <summary>
        /// Estado de la entrega
        /// </summary>
        [Required]
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Tipo de estado de la entrega
    /// </summary>
    public enum StateDelivery
    {
        /// <summary>
        /// Cerrada
        /// </summary>
        Close, //0
        /// <summary>
        /// Abierta
        /// </summary>
        Open, //1
        /// <summary>
        /// Abierta
        /// </summary>
        Error, //2
    }
}
