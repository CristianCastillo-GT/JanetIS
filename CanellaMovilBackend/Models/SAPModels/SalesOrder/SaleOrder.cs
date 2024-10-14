using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.SalesOrder
{
    /// <summary>
    /// Encabezado de una Orden de Ventas (ORDR)
    /// </summary>
    public class SaleOrder
    {
        /// <summary>
        /// CardCode - Código del cliente
        /// </summary>
        [Required]
        public string CardCode { get; set; } = string.Empty;
        /// <summary>
        /// DocDate - Fecha del contabilización
        /// </summary>
        [Required]
        public DateTime DocDate { get; set; }

        /// <summary>
        /// DocDueDate - Válido hasta
        /// </summary>
        [Required]
        public DateTime DocDueDate { get; set; }

        /// <summary>
        /// TaxDate - Fecha del documento 
        /// </summary>
        [Required]
        public DateTime TaxDate { get; set; }

        /// <summary>
        /// Serie del documento
        /// </summary>
        [Required]
        public int Series { get; set; }

        /// <summary>
        /// Código de la persona de ventas
        /// </summary>
        [Required]
        public int SalesPersonCode { get; set; }

        /// <summary>
        /// Código de la persona propietaria
        /// </summary>
        [Required]
        public int DocumentsOwner { get; set; }

        /// <summary>
        /// Comments - Comentarios
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /* CAMPOS DE USUARIO  */

        /// <summary>
        /// Usuario Autorizador
        /// </summary>
        [Required]
        public string U_Autorizador { get; set; } = string.Empty;

        /// <summary>
        /// la llamada de servicio relacionada
        /// </summary>
        [Required]
        public string U_LlamadaServicio { get; set; } = string.Empty;

        /// <summary>
        /// NIT del cliente
        /// </summary>
        [Required]
        public string U_SNNit { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        [Required]
        public string U_SNNombre { get; set; } = string.Empty;

        /// <summary>
        /// El documento hace referencia si es una llamada de servico
        /// </summary>
        [Required]
        public string U_DoctoGenServ { get; set; } = string.Empty;

        /// <summary>
        /// El usuario solicitante
        /// </summary>
        [Required]
        public string U_Solicitante { get; set; } = string.Empty;

        /// <summary>
        /// El número de contratos
        /// </summary>
        public string? U_Contrato { get; set; }

        /// <summary>
        /// El número de contratos de mantenimiento
        /// </summary>
        public string? U_ContratoMant { get; set; }

        /// <summary>
        /// El número de contratos de mantenimiento
        /// </summary>
        [Required]
        public string U_Requisicion { get; set; } = string.Empty;

        /* DETALLE DEL DOCUMENTO */

        /// <summary>
        /// AddessesList - Listado de direcciones
        /// </summary>
        [Required]
        public List<SaleOrderDetail>? Items { get; set; }
    }
}
