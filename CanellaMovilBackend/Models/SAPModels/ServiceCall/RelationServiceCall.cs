using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.ServiceCall
{
    /// <summary>
    /// Relación entre una llamada de servicio y un documento en SAP
    /// </summary>
    public class RelationServiceCall
    {
        /// <summary>
        /// callID - ID de una llamada de servicio
        /// </summary>
        [Required]
        public int CallID { get; set; }

        /// <summary>
        /// DocEntrySaleOrder - DocEntry de una orden de venta
        /// </summary>
        [Required]
        public int DocEntrySaleOrder { get; set; }
    }
}
