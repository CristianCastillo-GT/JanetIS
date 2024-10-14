using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo de datos para la petición de inventario de pagina canon
    /// </summary>
    public class INRequestData
    {
        /// <summary>
        /// Modelo de datos para la petición de inventario de cliente de pagina canon
        /// </summary>
        public class RequestGetInventory
        {
            /// <summary>
            /// Nit del cliente
            /// </summary>
            public string AddId { get; set; } = string.Empty;


        }
    }
}
