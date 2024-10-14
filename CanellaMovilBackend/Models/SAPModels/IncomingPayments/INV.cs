namespace CanellaMovilBackend.Models.SAPModels.IncomingPayments
{
    /// <summary>
    /// Objeto de Facturas
    /// </summary>
    public class INV
    {
        /// <summary>
        /// Factura
        /// </summary>
        public string NumAtCard { get; set; }
        /// <summary>
        /// Cantidad deudora
        /// </summary>
        public string SumApplied {  get; set; }
    }
}
