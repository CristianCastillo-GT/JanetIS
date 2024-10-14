using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo de datos para la petición de ordenes del cliente de pagina canon
    /// </summary>
    public class SORequestData
    {
        /// <summary>
        /// Modelo de datos para la petición de ordenes del cliente de pagina canon
        /// </summary>
        public class RequestGetCardCode
        {
            /// <summary>
            /// Nit del cliente
            /// </summary>
            public string AddId { get; set; } = string.Empty;


        }
    }
}
