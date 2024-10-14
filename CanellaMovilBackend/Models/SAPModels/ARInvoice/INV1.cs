using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.ARInvoice
{
    /// <summary>
    /// Detalle de una Orden de Ventas(OQUT)
    /// </summary>
    public class INV1
    {
        /* CAMPOS NATIVOS SAP */

        /// <summary>
        /// DocEntry - ID interno del documento
        /// </summary>
        public string DocEntry { get; set; } = string.Empty;

        /// <summary>
        /// DocDate - Numero de artículo
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// Dscription - Descripción del artículo
        /// </summary>
        public string Dscription { get; set; } = string.Empty;

        /// <summary>
        /// Quantity - Cantidad
        /// </summary>
        public string Quantity { get; set; } = string.Empty;
        /// <summary>
        /// PriceAfterVAT - Precio tras Descuento
        /// </summary>
        public string PriceAfVAT { get; set; } = string.Empty;
        /// <summary>
        /// DiscountPercent - Porcentaje de descuento
        /// </summary>
        public string DiscPrcnt { get; set; } = string.Empty;
        /// <summary>
        /// UnitPrice - Precio por unidad
        /// </summary>
        public string PriceItem { get; set; } = string.Empty;
        /// <summary>
        /// LineTotal - Total de linea
        /// </summary>
        public string LineTotal { get; set; } = string.Empty;
        /// <summary>
        /// Currency - Moneda
        /// </summary>
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// WarehouseCode - Almacén
        /// </summary>
        public string Whscode { get; set; } = string.Empty;
        /// <summary>
        /// SalesPersonCode - Empleado de Ventas
        /// </summary>
        public string SlpCode { get; set; } = string.Empty;
        /// <summary>
        /// CostingCode - Centro de Costo
        /// </summary>
        public string OcrCode { get; set; } = string.Empty;
        /// <summary>
        /// TaxCode - Indicador de Impuestos
        /// </summary>
        public string TaxCode { get; set; } = string.Empty;
        /// <summary>
        /// FreeTxt - Comentario en linea de artículo
        /// </summary>
        public string FreeTxt { get; set; } = string.Empty;
        /// <summary>
        /// COGSCostingCode(OcrCode) - Norma de reparto de precios de coste
        /// </summary>
        public string COGSCostingCode { get; set; } = string.Empty;

        /* CAMPOS DE USUARIO */

        /// <summary>
        /// U_Solicitado - Cantidad Solicitada
        /// </summary>
        public string U_Solicitado { get; set; } = string.Empty;
        /// <summary>
        /// U_Tipo - Tipo de Compra/Venta
        /// </summary>
        public string U_Tipo { get; set; } = string.Empty;
    }
}
