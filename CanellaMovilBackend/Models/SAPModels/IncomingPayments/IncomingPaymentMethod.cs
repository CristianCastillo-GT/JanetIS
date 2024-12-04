namespace CanellaMovilBackend.Models.SAPModels.IncomingPayments
{

    /// <summary>
    /// Tipo de Pago
    /// </summary>
    public class IncomingPaymentMethod
    {

        /// <summary>
        /// Fecha del documento
        /// </summary>
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// Referencia
        /// </summary>
        public string ReferenceNumber { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de Pago
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Codigo de Pais
        /// </summary>
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Codigo Bancario
        /// </summary>
        public string BankCode { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de Cheque P = Personal y E = Empresa
        /// </summary>
        public string U_TipoCK { get; set; } = string.Empty;

        /// <summary>
        /// Suma
        /// </summary>
        public string Sum { get; set; } = string.Empty;

    }
}
