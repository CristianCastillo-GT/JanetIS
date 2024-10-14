using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo de datos para la petición de dashboard de cliente de pagina canon
    /// </summary>
    public class DBRequestData
    {
        /// <summary>
        /// Modelo de datos para la petición de dashboard de cliente de pagina canon
        /// </summary>
        public class RequestGetDashBoard
        {
            /// <summary>
            /// Nit del cliente
            /// </summary>
            public string AddId { get; set; } = string.Empty;


        }
    }
}
