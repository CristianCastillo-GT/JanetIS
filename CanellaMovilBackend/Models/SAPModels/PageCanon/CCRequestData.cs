using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo de datos para la petición de consulta de cliente de pagina canon
    /// </summary>
    public class CCRequestData
    {
        /// <summary>
        /// Modelo de datos para la petición de consulta de cliente de pagina canon
        /// </summary>
        public class RequestGetSalesOrder
        {
            /// <summary>
            /// Nit del cliente
            /// </summary>
            public string AddId { get; set; } = string.Empty;


        }
    }
}
