namespace CanellaMovilBackend.Models.SAPModels.SalesQuotation
{
    /// <summary>
    /// Modelo de datos para la petición de consulta de cliente
    /// </summary>
    public class RequestDataQuotation
    {
        /// <summary>
        /// Modelo de datos para la petición de consulta de Oferta de Ventas
        /// </summary>
        public class RequestGetQuotation
        {
            /// <summary>
            /// DocNum del la Oferta de venta
            /// </summary>
            public string DocNum { get; set; } = string.Empty;
        }
    }
}
