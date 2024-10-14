using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.Bank_Statements_and_External_Reconciliations
{
    /// <summary>
    /// Modelo para la trata de estados de cuenta externos
    /// </summary>
    public class OBNK
    {
        /// <summary>
        /// Code - Codigo de Operación
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// DueDate - Fecha
        /// </summary>
        public string DueDate { get; set; } = string.Empty;

        /// <summary>
        /// AcctCode - Cuenta Bancaria
        /// </summary>
        public string AcctCode { get; set; } = string.Empty;

        /// <summary>
        /// Ref - Referencia
        /// </summary>
        public string Ref { get; set; } = string.Empty;

        /// <summary>
        /// Memo - Descripcion
        /// </summary>
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// DebAmount - Debito
        /// </summary>
        public string DebAmount { get; set; } = string.Empty;

        /// <summary>
        /// CredAmnt - Credito
        /// </summary>
        public string CredAmnt { get; set; } = string.Empty;
    }
}
