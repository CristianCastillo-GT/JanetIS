using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo de datos para la petición del detalle de articulo de pagina canon
    /// </summary>
    public class IDRequestData
    {

        /// <summary>
        /// Modelo de datos para la petición de inventario de cliente de pagina canon
        /// </summary>
        public class RequestGetInventoryDetail
        {
            /// <summary>
            /// Nit del cliente
            /// </summary>
            public string AddId { get; set; } = string.Empty;

            /// <summary>
            /// codigo de articulo
            /// </summary>
            public string ItemCode { get; set; } = string.Empty;


        }
       
    }
}
