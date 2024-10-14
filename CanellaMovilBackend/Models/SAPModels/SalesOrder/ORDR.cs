using CanellaMovilBackend.Models.SAPModels.BusinessPartner;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.SalesOrder
{
    /// <summary>
    /// Encabezado de una Orden de Ventas (ORDR)
    /// </summary>
    public class ORDR
    {
        /* CAMPOS NATIVOS SAP  */

        /// <summary>
        /// DocDate - Fecha del contabilización
        /// </summary>
        [Required]
        public string DocDate { get; set; } = string.Empty;

        /// <summary>
        /// DocNum - Identificador en cliente SAP
        /// </summary>
        public string DocNum { get; set; } = string.Empty;

        /// <summary>
        /// DocEntry - Identificador en SAP BD
        /// </summary>
        public string DocEntry { get; set; } = string.Empty;

        /// <summary>
        /// DocDueDate - Válido hasta
        /// </summary>
        [Required]
        public string DocDueDate { get; set; } = string.Empty;

        /// <summary>
        /// TaxDate - Fecha del documento 
        /// </summary>
        public string TaxDate { get; set; } = string.Empty;

        /// <summary>
        /// CardCode - Código del cliente
        /// </summary>
        public string CardCode { get; set; } = string.Empty;

        /// <summary>
        /// CardName - Nombre del Cliente
        /// </summary>
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// NumAtCard - Referencia
        /// </summary>
        public string NumAtCard { get; set; } = string.Empty;
        /// <summary>
        /// CANCELED - Documento cancelado Y/N
        /// </summary>
        public string CANCELED { get; set; } = string.Empty;
        /// <summary>
        /// DocCurrency - Moneda del documento
        /// </summary>
        public string DocCur { get; set; } = string.Empty;
        /// <summary>
        /// Comments - Comentarios
        /// </summary>
        public string Comments { get; set; } = string.Empty;
        /// <summary>
        /// Series - Serie del documento
        /// </summary>
        public string Series { get; set; } = string.Empty;
        /// <summary>
        /// SalesPersonCode - Empleado de ventas
        /// </summary>
        public string SlpCode { get; set; } = string.Empty;
        /// <summary>
        /// GroupNumber - Condición de pago
        /// </summary>
        public string GroupNumber { get; set; } = string.Empty;

        /// <summary>
        /// DocStatus - Estado del Documento
        /// </summary>
        public string DocStatus { get; set; } = string.Empty;

        /* CAMPOS DE USUARIO  */

        /// <summary>
        /// U_DoctoFiscal - Documento Fiscal
        /// </summary>
        public string U_DoctoFiscal { get; set; } = string.Empty;
        /// <summary>
        /// U_NoOrdenCompra - No. Orden de Compra
        /// </summary>
        public string U_NoOrdenCompra { get; set; } = string.Empty;
        /// <summary>
        /// U_DoctoRef - Documento de Referencia
        /// </summary>
        public string U_DoctoRef { get; set; } = string.Empty;
        /// <summary>
        /// U_DoctoRefNo - No. de Documento de Referencia
        /// </summary>
        public string U_DoctoRefNo { get; set; } = string.Empty;
        /// <summary>
        /// U_DoctoGenServ - Generado por orden de servicio
        /// </summary>
        public string U_DoctoGenServ { get; set; } = string.Empty;
        /// <summary>
        /// U_MotivoDesc - Motivo de Descuento
        /// </summary>
        public string U_MotivoDesc { get; set; } = string.Empty;
        /// <summary>
        /// U_SNNit - Nit o DPI
        /// </summary>
        public string U_SNNit { get; set; } = string.Empty;
        /// <summary>
        /// U_SNNombre - Nombre del Socio de Negocios
        /// </summary>
        public string U_SNNombre { get; set; } = string.Empty;
        /// <summary>
        /// U_PersonaContacto - Nombre Persona de Contacto
        /// </summary>
        public string U_PersonaContacto { get; set; } = string.Empty;
        /// <summary>
        /// U_PersonaTel - Teléfono de la persona de contacto
        /// </summary>
        public string U_PersonaTel { get; set; } = string.Empty;
        /// <summary>
        /// U_PersonaCorreo - Correo de contacto
        /// </summary>
        public string U_PersonaCorreo { get; set; } = string.Empty;

        /* DETALLE DEL DOCUMENTO */

        /// <summary>
        /// AddessesList - Listado de direcciones
        /// </summary>
        [Required]
        public List<RDR1>? Items { get; set; }

        /// <summary>
        /// AddessesList
        /// </summary>
        public ORDR()
        {
            Items = new List<RDR1>();
        }
    }
}