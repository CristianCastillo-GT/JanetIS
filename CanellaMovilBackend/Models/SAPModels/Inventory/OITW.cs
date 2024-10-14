using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.Inventory
{
    /// <summary>
    /// Modelo de Socio de Negocios SAP
    /// </summary>
    public class OITW
    {
        /// <summary>
        /// ItemCode - Codigo de Item
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// WhsCode - Codigo de la bodega
        /// </summary>
        public string WhsCode { get; set; } = string.Empty;

        /// <summary>
        /// OnHand - En Stock
        /// </summary>
        public string OnHand { get; set; } = string.Empty;

    }
}
