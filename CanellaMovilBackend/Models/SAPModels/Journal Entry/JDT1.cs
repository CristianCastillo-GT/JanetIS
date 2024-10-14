namespace CanellaMovilBackend.Models.SAPModels.Journal_Entry
{
    /// <summary>
    /// Detalle del asiento contable
    /// </summary>
    public class JDT1
    {
        /// <summary>
        /// Codigo de la cuenta contable
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// Monto debito
        /// </summary>
        public string Debit { get; set; } = string.Empty;

        /// <summary>
        /// Monto credito
        /// </summary>
        public string Credit { get; set; } = string.Empty;

        /// <summary>
        /// Comentario
        /// </summary>
        public string LineMemo { get; set; } = string.Empty;
    }
}
