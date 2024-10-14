using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo de de ordenes asociadas a un clinte SAP Canon
    /// </summary>
    public class SalesOrder
    {
        /// <summary>
        /// DocNum - Numero de documento de sap
        /// </summary>
        public string DocNum { get; set; } = string.Empty;

        /// <summary>
        /// Code - Codigo de Cliente
        /// </summary>
        public string CardCode { get; set; } = string.Empty;

        /// <summary>
        /// Name / Nombre o Razón Social
        /// </summary>
        [Required]
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// DocEstatus - Estado del documento de SAP
        /// </summary>
        [Required]
        public string DocEstatus { get; set; } = string.Empty;

        /// <summary>
        /// DocTotal - Total del documento de SAP
        /// </summary>
        [Required]
        public string DocTotal { get; set; } = string.Empty;

        /// <summary>
        /// DocDate - Fecha del documento de SAP
        /// </summary>
        [Required]
        public string DocDate { get; set; } = string.Empty;
    }
}
