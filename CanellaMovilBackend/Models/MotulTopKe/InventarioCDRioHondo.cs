using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.MotulTopKe
{
    /// <summary>
    /// Modelo de consulta para obtener inventario SAP canella CD Rio Hondo 
    /// </summary>
    public class InventarioCDRioHondo
    {
        /// <summary>
        /// ItemCode - Codigo de articulo
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;
        /// <summary>
        /// ItemName - Nombre de articulo
        /// </summary>
        public string ItemName { get; set; } = string.Empty;
        /// <summary>
        /// WhsCode - Codigo de Bodega
        /// </summary>
        public string WhsCode { get; set; } = string.Empty;
        /// <summary>
        /// WhsName - Nombre de Bodega
        /// </summary>
        public string WhsName { get; set; } = string.Empty;
        /// <summary>
        /// Disponible - Cantidad Disponible del Articulo
        /// </summary>
        public string Disponible { get; set; } = string.Empty;
    }
}
