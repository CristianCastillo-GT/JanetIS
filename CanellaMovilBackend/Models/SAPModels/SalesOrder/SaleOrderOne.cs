using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.SalesOrder
{
    /// <summary>
    /// Documento para obtener la orden de venta
    /// </summary>
    public class SaleOrderOne
    {
        /// <summary>
        /// DocNum - Identificador en cliente SAP
        /// </summary>
        [Required]
        public int DocNum { get; set; }

        /// <summary>
        /// DocEntry - Identificador en SAP BD
        /// </summary>
        [Required]
        public int DocEntry { get; set; }

        /// <summary>
        /// DocDate - Fecha del contabilización
        /// </summary>
        [Required]
        public string DocDate { get; set; } = string.Empty;

        /// <summary>
        /// CardCode - Código del cliente
        /// </summary>
        [Required]
        public string CardCode { get; set; } = string.Empty;

        /// <summary>
        /// CardName - Nombre del Cliente
        /// </summary>
        [Required]
        public string CardName { get; set; } = string.Empty;
        /// <summary>
        /// CANCELED - Documento cancelado Y=CANCELADO/N=NO CANCELADO
        /// </summary>
        [Required]
        public string CANCELED { get; set; } = string.Empty;

        /// <summary>
        /// DocStatus - Estado de la orden de venta C=Cerrado/O=Abierta
        /// </summary>
        [Required]
        public string DocStatus { get; set; } = string.Empty;
    }
}
