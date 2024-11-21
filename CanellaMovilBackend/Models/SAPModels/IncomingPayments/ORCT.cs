using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.OutgoingPayments;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.IncomingPayments
{
    /// <summary>
    /// Objeto de depositos
    /// </summary>
    public class ORCT
    {

        // Encabezado

        /// <summary>
        /// DocType / Tipo de Documento A=Account, C=Customer, D=TDS, P=P.L.A, S=Vendor, T=Tax
        /// </summary>
        public string DocType { get; set; } = string.Empty;

        /// <summary>
        /// DocDate / Fecha de Creacion del Documento
        /// </summary>
        public string DocDate { get; set; } = string.Empty;

        /// <summary>
        /// DocDueDate / Fecha Expiracion
        /// </summary>
        public string DocDueDate { get; set; } = string.Empty;

        /// <summary>
        /// TaxDate / Fecha del documento
        /// </summary>
        public string TaxDate { get; set; } = string.Empty;

        /// <summary>
        /// CardCode / Codigo del cliente
        /// </summary>
        public string? CardCode { get; set; } = string.Empty;

        /// <summary>
        /// CardCode / Codigo del cliente
        /// </summary>
        public string? CardName { get; set; } = string.Empty;

        /// <summary>
        /// DocCurr / Moneda
        /// </summary>
        public string DocCurr { get; set; } = string.Empty;

        /// <summary>
        /// DocRate / Tipo de Cambio
        /// </summary>
        public string DocRate { get; set; } = string.Empty;

        /// <summary>
        /// CashAcct / Cuenta Efectivo
        /// </summary>
        public string CashAcct { get; set; } = string.Empty;

        /// <summary>
        /// ControlAccount / Cuenta de Control
        /// </summary>
        public string BpAct { get; set; } = string.Empty;

        /// <summary>
        /// CheckAccount / Cuenta Cheques
        /// </summary>
        public string CheckAccount { get; set; } = string.Empty;

        /// <summary>
        /// TransferAccount / Cuenta de Transferencia
        /// </summary>
        public string TransferAccount { get; set; } = string.Empty;

        /// <summary>
        /// CashSum / Suma de Efectivo
        /// </summary>
        public string CashSum { get; set; } = string.Empty;

        /// <summary>
        /// CheckSum / Suma de Cheques
        /// </summary>
        public string CheckSum { get; set; } = string.Empty;

        /// <summary>
        /// TrsfrSum / Suma de Transferencia
        /// </summary>
        public string TrsfrSum { get; set; } = string.Empty;

        /// <summary>
        /// Comments / Comentarios
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        //Campos de usuario

        /// <summary>
        /// U_OCDocNum / Numero de Orden de compra
        /// </summary>
        public string U_OCDocNum { get; set; } = string.Empty;

        /// <summary>
        /// U_TipoPagos 
        /// </summary>
        public string U_TipoPagos { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de pago
        /// </summary>
        public string U_TipoPago { get; set; } = string.Empty;

        /// <summary>
        /// Referencia / Numero de Referencia
        /// </summary>
        public string? CounterRef { get; set; } = string.Empty;

        /// <summary>
        /// Referencia / Numero de Referencia en asiento contable
        /// </summary>
        public string? Ref1 { get; set; } = string.Empty;

        /// <summary>
        /// Referencia / Número de la Boleta
        /// </summary>
        public string? U_DpsBoleta { get; set; } = string.Empty;

        /// <summary>
        /// Referencia / Código del Banco a realizar el depósito
        /// </summary>
        public string? U_BcoDpsBoleta { get; set; } = string.Empty;


        /// <summary>
        /// Comentario / Comentarios
        /// </summary>
        public string? JrnlMemo { get; set; } = string.Empty;

        /// <summary>
        /// Vendedor
        /// </summary>
        public int U_EmpCode { get; set; }

        /// <summary>
        /// Vendedor
        /// </summary>
        public int U_Cobrador { get; set; }

        /// <summary>
        /// VoucherList / Listado de direcciones
        /// </summary>
        //[Required]
        public List<IncomingPaymentMethod>? IncomingPaymentMethod { get; set; }

        /// <summary>
        /// Account list / Listado de Cuentas
        /// </summary>
        
        public List<RCT4>? RCT4 { get; set; }

        /// <summary>
        /// Listado de tarjetas
        /// </summary>
        //[Required]
        public List<RCT3>? RCT3 { get; set; }

        /// <summary>
        /// Listado de Facturas
        /// </summary>
        public List<INV>? INV { get; set; }

        /// <summary>
        /// Errores de SAP
        /// </summary>
        public string? ErrorSAP { get; set; }
        
        /// <summary>
        /// Mensaje de Exito 
        /// </summary>
        public string? MensajeExito {  get; set; }

        /// <summary>
        /// VoucherList
        /// </summary>
        public ORCT()
        {
            IncomingPaymentMethod = [];
        }

    }
}
