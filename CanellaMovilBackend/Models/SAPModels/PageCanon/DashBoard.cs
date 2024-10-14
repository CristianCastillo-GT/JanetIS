using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Clase
    /// </summary>
    public class DashBoard
    {
        /// <summary>
        ///  Name / Nombre o Razón Social
        /// </summary>
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// Code - Codigo de Cliente
        /// </summary>
        [Required]
        public string CardCode { get; set; } = string.Empty;

        /// <summary>
        /// InvoiceOpen - Facturas abiertas del cliente
        /// </summary>
        [Required]
        public string InvoiceOpen { get; set; } = string.Empty;

        /// <summary>
        /// OrdersPendingDelivery - Ordenes pendientes de entrega del cliente
        /// </summary>
        [Required]
        public string OrdersPendingDelivery { get; set; } = string.Empty;

        /// <summary>
        /// AvailableCredit - Credito disponible
        /// </summary>
        [Required]
        public string AvailableCredit { get; set; } = string.Empty;
    }
}
