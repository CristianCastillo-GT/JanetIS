using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.Inventory
{
    /// <summary>
    /// Modelo de datos para la petición de consulta de cliente
    /// </summary>
    public class RequestDataInventory
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

        /// <summary>
        /// Modelo de datos para la petición de consulta de Item
        /// </summary>
        public class RequestGetItem
        {
            /// <summary>
            /// ItemCode de un item
            /// </summary>
            public string ItemCode { get; set; } = string.Empty;


        }

        /// <summary>
        /// Modelo de datos para la petición de consulta masiva de Item por rango de fecha
        /// </summary>
        public class RequestGetItemMassive
        {
            /// <summary>
            /// Fecha inicial
            /// </summary>
            public string InitialDate { get; set; } = string.Empty;

            /// <summary>
            /// Fecha final
            /// </summary>
            public string FinalDate { get; set; } = string.Empty;


        }

        /// <summary>
        /// Modelo de datos para la petición de consulta masiva de Item por rango de fecha y división del articulo
        /// </summary>
        public class RequestGetItemByDivisionMassive
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
            /// División
            /// </summary>
            [Required(ErrorMessage = "Debe ingresar la division del articulo .")]
            public string Division { get; set; } = string.Empty;

        }
    }
}
