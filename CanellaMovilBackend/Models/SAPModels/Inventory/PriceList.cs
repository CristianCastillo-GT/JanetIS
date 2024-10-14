using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.Inventory
{
    /// <summary>
    /// Modelo que captura el listado de precios
    /// </summary>
    public class PriceList
    {
        /// <summary>
        /// ItemCode - Codigo de Producto
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// ListName - Nombre de la lista de precios
        /// </summary>
        public string ListName { get; set; } = string.Empty;

        /// <summary>
        /// Price - Precio
        /// </summary>
        public string Price { get; set; } = string.Empty;

        /// <summary>
        /// AvgPrice - Costo
        /// </summary>
        public string AvgPrice { get; set; } = string.Empty;

    }
}
