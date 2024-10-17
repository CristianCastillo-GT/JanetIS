using CanellaMovilBackend.Models.CQMModels;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.OutgoingPayments
{
    /// <summary>
    /// Objeto de depositos
    /// </summary>
    public class OVPM
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
        /// Tipo de pago
        /// </summary>
        public string U_TipoPago { get; set; } = "3";

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
        /// U_TipoPagos / Tipo de pago
        /// </summary>
        public string U_TipoPagos { get; set; } = string.Empty;

        /// <summary>
        /// Referencia / Numero de Referencia
        /// </summary>
        public string? CounterRef { get; set; } = string.Empty;

        /// <summary>
        /// Referencia / Numero de Referencia en asiento contable
        /// </summary>
        public string? Ref2 { get; set; } = string.Empty;

        /// <summary>
        /// Comentario / Comentarios
        /// </summary>
        public string? JrnlMemo { get; set; } = string.Empty;

        /// <summary>
        /// VoucherList / Listado de direcciones
        /// </summary>
        //[Required]
        public List<PaymentMethod>? PaymentMethod { get; set; }

        /// <summary>
        /// Account list / Listado de Cuentas
        /// </summary>
        //[Required]
        public List<VPM4>? VPM4 { get; set; }




        /// <summary>
        /// VoucherList
        /// </summary>
        public OVPM()
        {
            PaymentMethod = new List<PaymentMethod>();
        }

    }
}
