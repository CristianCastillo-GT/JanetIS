namespace CanellaMovilBackend.Models.SAPModels.IncomingPayments
{

    /// <summary>
    /// Detalle de pagos a cuenta
    /// </summary>
    public class RCT4
    {
        /// <summary>
        /// Cuenta ISR
        /// </summary>
        public string AccountCode { get; set; } = string.Empty;

        /// <summary>
        /// nombre de cuenta 
        /// </summary>
        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// comentario  
        /// </summary>
        public string Decription { get; set; } = string.Empty;

        /// <summary>
        /// total de pago  
        /// </summary>
        public double SumPaid { get; set; }

    }
}
