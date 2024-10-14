using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.BusinessPartner
{
    /// <summary>
    /// Modelo de Socio de Negocios SAP
    /// </summary>
    public class OCRD
    {
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
        /// Foreign Name - Nombre Comercial
        /// </summary>
        [Required]
        public string CardFName { get; set; } = string.Empty;

        /// <summary>
        /// Group - Grupo Comercial
        /// </summary>
        [Required]
        public string GroupCode { get; set; } = string.Empty;

        /// <summary>
        /// Group - Condiciones de Pago
        /// </summary>
        [Required]
        public string GroupNum { get; set; } = string.Empty;

        /// <summary>
        /// BillToDef - Dirección principal Fiscal
        /// </summary>
        public string BillToDef { get; set; } = string.Empty;

        /// <summary>
        /// ShipToDef - Dirección principal Entrega
        /// </summary>
        public string ShipToDef { get; set; } = string.Empty;

        /// <summary>
        /// Balance - Saldo de Cuenta
        /// </summary>
        public string Balance { get; set; } = string.Empty;

        /// <summary>
        /// DNotesBal - Entregas
        /// </summary>
        public string DNotesBal { get; set; } = string.Empty;

        /// <summary>
        /// OrdersBal - Pedidos Clientes
        /// </summary>
        public string OrdersBal { get; set; } = string.Empty;


        /// <summary>
        /// Unified Federal Tax ID - DPI o Pasaporte
        /// </summary>
        [Required]
        public string VatIdUnCmp { get; set; } = string.Empty;

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
        /// CURP - NIT
        /// </summary>
        [Required]
        public string AddID { get; set; } = string.Empty;

        /// <summary>
        /// Credit Limit - Limite de Credito
        /// </summary>
        public string CreditLine { get; set; } = string.Empty;

        /// <summary>
        /// Cobrador - Cobrador de calle
        /// </summary>
        public string U_CobradorCode { get; set; } = string.Empty;

        /// <summary>
        /// AddessesList - Listado de direcciones
        /// </summary>
        [Required]
        public List<CRD1>? Addresses { get; set; } = new List<CRD1>();

        /// <summary>
        /// ContactList - Listado de Contactos
        /// </summary>
        public List<OCPR>? Contacts { get; set; } = new List<OCPR>();
    }
}
