using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.SalesOrder
{
    /// <summary>
    /// Detalle de una Orden de Ventas(OQUT)
    /// </summary>
    public class SaleOrderDetail
    {
        /// <summary>
        /// DocDate - Numero de artículo
        /// </summary>
        [Required]
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// Quantity - Cantidad
        /// </summary>
        [Required]
        public double Quantity { get; set; }
        /// <summary>
        /// UnitPrice - Precio
        /// </summary>
        [Required]
        public double UnitPrice { get; set; }

        /// <summary>
        /// DiscountPercent - Porcentaje de descuento
        /// </summary>
        [Required]
        public double DiscountPercent { get; set; }

        /// <summary>
        /// WarehouseCode - Código de bodega
        /// </summary>
        [Required]
        public string WarehouseCode { get; set; } = string.Empty;

        /// <summary>
        /// TaxCode - Código de impuesto
        /// </summary>
        [Required]
        public string TaxCode { get; set; } = string.Empty;

        /// <summary>
        /// CostingCode - Centro de Costo
        /// </summary>
        [Required]
        public string CostingCode { get; set; } = string.Empty;

        /// <summary>
        /// COGSCostingCode - Centro de Costo
        /// </summary>
        [Required]
        public string COGSCostingCode { get; set; } = string.Empty;

        /// <summary>
        /// U_Tipo - Tipo de compra (Bien/Servicio)
        /// </summary>
        [Required]
        public string U_Tipo { get; set; } = string.Empty;
    }
}
