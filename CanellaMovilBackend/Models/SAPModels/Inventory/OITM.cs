using CanellaMovilBackend.Models.SAPModels.SalesQuotation;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.Inventory
{
    /// <summary>
    /// Modelo de Socio de Negocios SAP
    /// </summary>
    public class OITM
    {
        /// <summary>
        /// Nombre del Producto
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// SellItem
        /// </summary>
        [Required]
        public string SellItem { get; set; } = string.Empty;

        /// <summary>
        /// PrchSeItem
        /// </summary>
        [Required]
        public string PrchSeItem { get; set; } = string.Empty;

        /// <summary>
        /// ItmsGrpCod
        /// </summary>
        [Required]
        public string ItmsGrpCod { get; set; } = string.Empty;


        /// <summary>
        /// ItemCode
        /// </summary>
        [Required]
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// Barcode
        /// </summary>
        [Required]
        public string Barcode { get; set; } = string.Empty;

        /// <summary>
        /// AvgPrice - Costo
        /// </summary>
        [Required]
        public string AvgPrice { get; set; } = string.Empty;

        /// <summary>
        /// OnHand
        /// </summary>
        [Required]
        public string OnHand { get; set; } = string.Empty;

        /// <summary>
        /// Transito
        /// </summary>
        [Required]
        public string Transito { get; set; } = string.Empty;

        /// <summary>
        /// Price
        /// </summary>
        [Required]
        public string Price { get; set; } = string.Empty;

        /// <summary>
        /// WhsList - Listado de direcciones
        /// </summary>
        public List<OITW>? WhsList { get; set; }

        /// <summary>
        /// PriceList - Listado de direcciones
        /// </summary>
        public List<PriceList>? PriceList { get; set; }

        /// <summary>
        /// AddessesList
        /// </summary>
        public OITM()
        {
            WhsList = new List<OITW>();
            PriceList = new List<PriceList>();
        }

    }
}
