namespace CanellaMovilBackend.Models.SAPModels.IncomingPayments
{
    /// <summary>
    /// Detalle de pagos con tarjeta credito
    /// </summary>
    public class RCT3
    {
        /// <summary>
        /// Nombre de Tarjeta de Credito
        /// </summary>
        public string CreditCard { get; set; } = String.Empty;
        /// <summary>
        /// Numero de Tarjeta de Credito
        /// </summary>
        public string CrCardNum { get; set; } = String.Empty;
        /// <summary>
        /// Fecha de expiracion
        /// </summary>
        public string CardValidUntil {  get; set; } = string.Empty;
        /// <summary>
        /// DPI o Licencia
        /// </summary>
        public string OwnerIdNum { get; set; } = String.Empty;
        /// <summary>
        /// Numero de Telefono
        /// </summary>
        public string OwnerPhone { get; set; } = String.Empty;
        /// <summary>
        /// Monto a pagar
        /// </summary>
        public string SumPaid {  get; set; } = String.Empty; 
        /// <summary>
        /// Fecha de Pago
        /// </summary>
        public string CrTypeCode { get; set; } = String.Empty;
        /// <summary>
        /// Voucher
        /// </summary>
        public string VoucherNum { get; set; } = String.Empty;
    }
}
