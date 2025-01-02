using CanellaMovilBackend.Models.SAPModels.Inventory;

namespace CanellaMovilBackend.Models.SAPModels.Journal_Entry
{
    /// <summary>
    /// Modelo de datos para los asientos contables
    /// </summary>
    public class OJDT
    {
        /// <summary>
        /// Fecha
        /// </summary>
        public string RefDate { get; set; } = string.Empty;

        /// <summary>
        /// Fecha
        /// </summary>
        public string DueDate { get; set; } = string.Empty;

        /// <summary>
        /// Fecha del documento
        /// </summary>
        public string TaxDate { get; set; } = string.Empty;

        /// <summary>
        /// Comentario
        /// </summary>
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// Referencia 1
        /// </summary>
        public string Ref1 { get; set; } = string.Empty;

        /// <summary>
        /// Referencia 2
        /// </summary>
        public string Ref2 { get; set; } = string.Empty;

        /// <summary>
        /// Referencia 3
        /// </summary>
        public string Ref3 { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de transacción
        /// </summary>
        public string TransCode { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de transacción
        /// </summary>
        public int Number { get; set; } 

        /// <summary>
        /// Detalle del asiento 
        /// </summary>
        public List<JDT1>? JournalLine { get; set; } = [];
    }
}
