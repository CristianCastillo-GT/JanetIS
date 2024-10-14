using CanellaMovilBackend.Models.CQMModels;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.Deposit
{
    /// <summary>
    /// Objeto de depositos
    /// </summary>
    public class ODPS
    {
        /// <summary>
        /// DeposCurr / Moneda del Deposito
        /// </summary>
        public string DeposCurr { get; set; } = string.Empty;

        /// <summary>
        /// DocRate / Tasa
        /// </summary>
        public string DocRate { get; set; } = string.Empty;

        /// <summary>
        /// DeposDate / Fecha del Deposito
        /// </summary>
        public string DeposDate { get; set; } = string.Empty;

        /// <summary>
        /// DeposAcct / Cuenta
        /// </summary>
        public string DeposAcct { get; set; } = string.Empty;

        /// <summary>
        /// BanckAcct / Cuenta Mayor
        /// </summary>
        public string BanckAcct { get; set; } = string.Empty;

        /// <summary>
        /// CrdBankAct / Cuenta de pago diferido
        /// </summary>
        public string CrdBankAct { get; set; } = string.Empty;

        /// <summary>
        /// Ref2 / Ref Banco (No.BoletaDEP)
        /// </summary>
        public string Ref2 { get; set; } = string.Empty;

        /// <summary>
        /// LocTotal / Cantidad
        /// </summary>
        //[Required]
        public string LocTotal { get; set; } = string.Empty;

        /// <summary>
        /// DepositType / Tipo de Deposito
        /// </summary>
        //[Required]
        public string DepositType { get; set; } = string.Empty;

        /// <summary>
        /// DpostorNam / Pagador
        /// </summary>
        public string DpostorNam { get; set; } = string.Empty;

        /// <summary>
        /// Memo / Comentarios
        /// </summary>
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// AllocAcct / Cuenta de Mayor (Detalle deposito tipo = CASH)
        /// </summary>
        public string AllocAcct { get; set; } = string.Empty;

        /// <summary>
        /// CommissionAccount / Cuenta de Comisión
        /// </summary>
        public string CommissionAccount { get; set; } = string.Empty;

        /// <summary>
        /// Commission / Importe de Comisión estándar
        /// </summary>
        public string Commision { get; set; } = string.Empty;

        /// <summary>
        /// TaxAccount / Cuenta de impuestos
        /// </summary>
        public string TaxAccount { get; set; } = string.Empty;

        /// <summary>
        /// TaxAmount / Monto del impuesto
        /// </summary>
        public string TaxAmount { get; set; } = string.Empty;


        /// <summary>
        /// CommissionDate / Fecha vencimiento comisión
        /// </summary>
        public string CommissionDate { get; set; } = string.Empty;

        /// <summary>
        /// VoucherList / Listado de direcciones
        /// </summary>
        //[Required]
        public List<string>? AbsId { get; set; }

        /// <summary>
        /// VoucherList
        /// </summary>
        public ODPS()
        {
            AbsId = new List<string>();
        }

    }
}
