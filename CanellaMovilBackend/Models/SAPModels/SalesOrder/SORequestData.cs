namespace CanellaMovilBackend.Models.SAPModels.SalesOrder
{
    /// <summary>
    /// Modelo de datos para la petición de consulta de cliente
    /// </summary>
    public class RequestDataOrder
    {
        /// <summary>
        /// Modelo de datos para la petición de consulta de Orden de Ventas
        /// </summary>
        public class RequestGetOrder
        {
            /// <summary>
            /// DocNum del la Orden de venta
            /// </summary>
            public string DocNum { get; set; } = string.Empty;
        }

        /// <summary>
        /// Modelo de datos para la petición de consulta de Orden de Ventas por vendedor
        /// </summary>
        public class RequestGetOrders
        {
            /// <summary>
            /// Fecha inicial
            /// </summary>
            public string InitialDate { get; set; } = string.Empty;

            /// <summary>
            /// Fecha final
            /// </summary>
            public string FinalDate { get; set; } = string.Empty;

            /// <summary>
            /// codigo del vendedor
            /// </summary>
            public string SlpCode { get; set; } = string.Empty;

            /// <summary>
            /// Estado del documento
            /// </summary>
            public string status { get; set; } = string.Empty;
        }
    }
}
