namespace CanellaMovilBackend.Models.SAPModels.IncomingPayments
{
    /// <summary>
    /// Facxturas
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Identificador interno de la factura (DocEntry en SAP).
        /// </summary>
        public int DocEntry { get; set; }

        /// <summary>
        /// Monto aplicado a esta factura.
        /// </summary>
        public string SumApplied { get; set; }

        /// <summary>
        /// Comentarios o referencia opcional.
        /// </summary>
        public string Comments { get; set; }
    }
}
