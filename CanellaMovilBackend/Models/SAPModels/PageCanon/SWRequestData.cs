using System.ComponentModel.DataAnnotations;


namespace CanellaMovilBackend.Models.SAPModels.PageCanon
{
    /// <summary>
    /// Modelo de datos para la petición de existencia por bodegas segun articulo y lista de precios del cliente de pagina canon
    /// </summary>
    public class SWRequestData
    {
        /// <summary>
        /// Modelo de datos para la petición de existencia por bodegas segun articulo y lista de precios del cliente de pagina canon
        /// </summary>
        public class RequestGetStockWhareHouse
        {
            /// <summary>
            /// Nit del cliente
            /// </summary>
            public string AddId { get; set; } = string.Empty;

            /// <summary>
            /// Codigo de articulo de sap
            /// </summary>
            public string ItemCode { get; set; } = string.Empty;


        }

    }
}
