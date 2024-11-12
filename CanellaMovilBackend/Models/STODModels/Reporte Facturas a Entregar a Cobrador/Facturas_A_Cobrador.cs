namespace CanellaMovilBackend.Models.STODModels.Reporte_Facturas_a_Entregar_a_Cobrador
{
    /// <summary>
    /// Obtener Facturas a Entregar a Cobrador
    /// </summary>
    public class Facturas_A_Cobrador
    {
        /// <summary>
        /// DocEntry
        /// </summary>
        public string DocEntry { get; set; }

        /// <summary>
        /// DocNum
        /// </summary>
        public string DocNum { get; set; }

        /// <summary>
        /// FacturaSerie
        /// </summary>
        public string FacturaSerie { get; set; }

        /// <summary>
        /// FacturaNumero
        /// </summary>
        public string FacturaNumero { get; set; }

        /// <summary>
        /// FacturaFecha
        /// </summary>
        public DateTime FacturaFecha { get; set; }

        /// <summary>
        /// ClienteCodigo
        /// </summary>
        public string ClienteCodigo { get; set; }

        /// <summary>
        /// ClienteNombre
        /// </summary>
        public string ClienteNombre { get; set; }

        /// <summary>
        /// FacturaTotal
        /// </summary>
        public decimal FacturaTotal { get; set; }

        /// <summary>
        /// CobradorCodigo
        /// </summary>
        public string CobradorCodigo { get; set; }

        /// <summary>
        /// CobradorNombre
        /// </summary>
        public string CobradorNombre { get; set; }

        /// <summary>
        /// Departamento
        /// </summary>
        public string Departamento { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// IND_Anulada
        /// </summary>
        public string IND_Anulada { get; set; }

        /// <summary>
        /// F_Marcado
        /// </summary>
        public DateTime F_Marcado { get; set; }

        /// <summary>
        /// Nota
        /// </summary>
        public string Nota { get; set; }

        /// <summary>
        /// Marcado_Usuario
        /// </summary>
        public string Marcado_Usuario { get; set; }

        /// <summary>
        /// SerieInterna
        /// </summary>
        public string SerieInterna { get; set; }

        /// <summary>
        /// CorrelativoInt
        /// </summary>
        public int CorrelativoInt { get; set; }
    }
}
