using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.PaginasWebModels
{
    /// <summary>
    /// Modelo de consulta para obtener inventario SAP canella y maquipos 
    /// </summary>
    public class InventarioMaquinaria
    {
        /// <summary>
        /// Empresa - Empresa de SAP
        /// </summary>
        public string Empresa { get; set; } = string.Empty;
        /// <summary>
        /// ItemCode - Codigo de articulo
        /// </summary>
        public string ItemCode { get; set; }= string.Empty;
        /// <summary>
        /// ItemName - Nombre de articulo
        /// </summary>
        public string ItemName {  get; set; }= string.Empty;
        /// <summary>
        /// Price - Precio de Articulo
        /// </summary>
        public string Price {  get; set; }= string.Empty;

    }
}
