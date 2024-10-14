using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo para mostrar inventario de canon
    /// </summary>
    public class Inventory
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
        /// Fotografia / Fotografia
        /// </summary>

        public string Fotografia { get; set; } = string.Empty;

        /// <summary>
        /// Stock - cantidad de articulos disponibles
        /// </summary>

        public string Stock { get; set; } = string.Empty;

        /// <summary>
        /// Price - precio segun lista de precio del cliente
        /// </summary>

        public string Price { get; set; } = string.Empty;

        /// <summary>
        /// CodeDivision - codigo de la division de SAP
        /// </summary>

        public string CodeDivision { get; set; } = string.Empty;

        /// <summary>
        /// NameDivision - Nombre de la division de SAP
        /// </summary>

        public string NameDivision { get; set; } = string.Empty;

        /// <summary>
        /// CodeCategoria - Codigo de la categoria de SAP
        /// </summary>

        public string CodeCategoria { get; set; } = string.Empty;

        /// <summary>
        /// NameCategoria - nombre de la categoria de SAP
        /// </summary>

        public string NameCategoria { get; set; } = string.Empty;

        /// <summary>
        /// CodeTipo - codigo del tipo de SAP
        /// </summary>

        public string CodeTipo { get; set; } = string.Empty;

        /// <summary>
        /// NameTipo - nombre del tipo de SAP
        /// </summary>
 
        public string NameTipo { get; set; } = string.Empty;


    }
}
