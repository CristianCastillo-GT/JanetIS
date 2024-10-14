using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.Reports
{
    /// <summary>
    /// Modelo de datos para la petición de consulta la cartera consolidada
    /// </summary>
    public class CCRequestData
    {
        /// <summary>
        /// Codigo del cliente en SAP
        /// </summary>
        public string CardCode { get; set; } = string.Empty;

        /// <summary>
        /// Modelo de datos para la petición de consulta la cartera consolidada
        /// </summary>
        public class RequestCarteCliente : CCRequestData
        {
        }

        /// <summary>
        /// Modelo de datos para la petición de consulta la cartera consolidada
        /// </summary>
        public class RequestPagosCliente : CCRequestData
        {
            /// <summary>
            /// fecha inicial
            /// </summary>
            public string FechaInicial { get; set; } = string.Empty;

            /// <summary>
            /// fecha final
            /// </summary>
            public string FechaFinal { get; set; } = string.Empty;

        }
        /// <summary>
        /// Modelo de datos para la petición de consulta del Estado promesa por cada json de promesa
        /// </summary>
        public class RequestPayPromise : CCRequestData
        {
            /// <summary>
            /// Fecha asignar el pago de la promesa
            /// </summary>
            public string fechaPromesa { get; set; } = string.Empty;

            /// <summary>
            /// monto que se asigna a la promesa
            /// </summary>
            public string amount { get; set; } = string.Empty;

            /// <summary>
            /// estado vigente
            /// </summary>
            public string status { get; set; } = string.Empty;

            /// <summary>
            /// fecha en que se creo la promesa
            /// </summary>
            public string fechaCreacion { get; set; } = string.Empty;

            /// <summary>
            /// id unico de la promesa
            /// </summary>
            public string idPromesa { get; set; } = string.Empty;

            /// <summary>
            /// docentry de la factura
            /// </summary>
            public string docentry { get; set; } = string.Empty;

            /// <summary>
            /// Para filtrar por documento
            /// </summary>
            public bool porDocumento { get; set; } = true;

        }
    }
}
