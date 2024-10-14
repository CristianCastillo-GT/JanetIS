using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.UsersTables
{
    /// <summary>
    /// Modelo de datos para la petición de tablas de usuario
    /// </summary>
    public class URequestData
    {
        /// <summary>
        /// Modelo de datos para la petición de Categoria de articulos
        /// </summary>
        public class RequestGetCategoria
        {
            /// <summary>
            /// CodeDivision es el codigo de la division que buscaremos
            /// </summary>
            public string CodeDivision { get; set; } = string.Empty;
            /// <summary>
            /// CodeTipo es el codigo del tipo que buscaremos
            /// </summary>
            public string CodeTipo { get; set; } = string.Empty;

        }

        /// <summary>
        /// Modelo de datos para la petición de division de articulos
        /// </summary>
        public class RequestGetDivision
        {
            /// <summary>
            /// CodeCategoria es el codigo de la categoria que buscaremos
            /// </summary>
            public string CodeCategoria { get; set; } = string.Empty;

            /// <summary>
            /// CodeTipo es el codigo del tipo que buscaremos
            /// </summary>
            public string CodeTipo { get; set; } = string.Empty;

        }

        /// <summary>
        /// Modelo de datos para la petición de tipo de articulos
        /// </summary>
        public class RequestGetTipo
        {
            /// <summary>
            /// CodeCategoria es el codigo de la categoria que buscaremos
            /// </summary>
            public string CodeCategoria { get; set; } = string.Empty;

            /// <summary>
            /// CodeDivision es el codigo de la division que buscaremos
            /// </summary>
            public string CodeDivision { get; set; } = string.Empty;

        }

    }
}
