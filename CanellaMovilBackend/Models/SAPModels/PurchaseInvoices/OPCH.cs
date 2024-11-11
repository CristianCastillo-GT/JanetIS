using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.PurchaseInvoices;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PurchaseInvoices
{

    /// <summary>
    /// Objeto de Factura Proveedores
    /// </summary>
    public class OPCH
    {


        // Encabezado

        /// <summary>
        /// Código de la Entrada de Mercancías
        /// </summary>
        public int ReceiptEntry { get; set; }

        /// <summary>
        /// Número de Serie de la Factura
        /// </summary>
        public string DocSerie { get; set; } = string.Empty;


        /// <summary>
        /// Número de la Factura
        /// </summary>
        public string DocNum { get; set; } = string.Empty;



    }
}
