namespace CanellaMovilBackend.Models.SAPModels.ARInvoice
{
    /// <summary>
    /// Modelo de datos para la petición de consulta de facturas
    /// </summary>
    public class RequestDataInvoice
    {
        /// <summary>
        /// Modelo de datos para la petición de consulta de facturas
        /// </summary>
        public class RequestGetInvoice
        {
            /// <summary>
            /// retorna las facturas de un cliente
            /// </summary>
            public string CardCode { get; set; } = string.Empty;
        }
    }
}
