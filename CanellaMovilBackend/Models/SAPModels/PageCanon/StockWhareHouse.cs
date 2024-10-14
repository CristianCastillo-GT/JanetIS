using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo de de existencia en bodegas por codigo de articulo y lista de precios asociadas a un clinte SAP Canon
    /// </summary>
    public class StockWhareHouse
    {
        /// <summary>
        /// ItemCode - codigo de articulo de sap
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// ItemName - nombre de articulo de sap
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// WhsCode / codigo de bodega
        /// </summary>
        [Required]
        public string WhsCode { get; set; } = string.Empty;

        /// <summary>
        /// WhsName - nombre de bodega
        /// </summary>
        [Required]
        public string WhsName { get; set; } = string.Empty;

        /// <summary>
        /// PriceList - Lista de precios asociada al nit campo de usuario U_PriceList1105
        /// </summary>
        [Required]
        public string PriceList { get; set; } = string.Empty;

        /// <summary>
        /// Stock - Existencia por bodega
        /// </summary>
        [Required]
        public string Stock { get; set; } = string.Empty;
    }
}
