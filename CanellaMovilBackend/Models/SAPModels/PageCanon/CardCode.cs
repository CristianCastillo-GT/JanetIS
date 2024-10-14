using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo de Socio de Negocios SAP Canon
    /// </summary>
    public class CardCode
    {
        /// <summary>
        /// Code - Codigo de Cliente
        /// </summary>
        public string CardCod { get; set; } = string.Empty;

        /// <summary>
        /// Name / Nombre o Razón Social
        /// </summary>
        [Required]
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// Tel 1 - Télefono 1
        /// </summary>
        [Required]
        public string Phone1 { get; set; } = string.Empty;

        /// <summary>
        /// Mobile Phone - Télefono Móvil
        /// </summary>
        [Required]
        public string Cellular { get; set; } = string.Empty;

        /// <summary>
        /// E-Mail - Correo electrónico
        /// </summary>
        [Required]
        public string E_Mail { get; set; } = string.Empty;

        /// <summary>
        /// AvailableCredit - Credito disponible
        /// </summary>
        [Required]
        public string AvailableCredit { get; set; } = string.Empty;

        /// <summary>
        /// AddID - NIT
        /// </summary>
        [Required]
        public string AddID { get; set; } = string.Empty;

        /// <summary>
        /// InvoiceOpen - Facturas abiertas segun nit
        /// </summary>
        [Required]
        public string InvoiceOpen { get; set; } = string.Empty;

    }
}
