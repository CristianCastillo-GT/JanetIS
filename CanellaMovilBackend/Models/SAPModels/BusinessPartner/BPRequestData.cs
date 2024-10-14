using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.BusinessPartner
{
    /// <summary>
    /// Modelo de datos para la petición de consulta de cliente
    /// </summary>
    public class RequestDataBusinessPartner
    {
        /// <summary>
        /// Modelo de datos para la petición de consulta de cliente
        /// </summary>
        public class RequestGetBusinessPartner
        {
            /// <summary>
            /// Codigo del cliente en SAP
            /// </summary>
            public string CardCode { get; set; } = string.Empty;

            /// <summary>
            /// Nit del cliente
            /// </summary>
            public string AddId { get; set; } = string.Empty;

            /// <summary>
            /// Identificador de la empresa
            /// </summary>
            public int Empresa { get; set; }

        }

        /// <summary>
        /// Modelo de datos para la petición de consulta masiva de Socios de Negocios en un rango de fechas
        /// </summary>
        public class RequestGetBusinessPartnerMassive
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
        /// Modelo de datos para la petición que actualiza el GroupNum de un socio de negocios
        /// </summary>
        public class RequestUpdateGroupNumPartner
        {
            /// <summary>
            /// Codigo del cliente en SAP
            /// </summary>
            public string CardCode { get; set; } = string.Empty;

            /// <summary>
            /// GroupCode del cliente
            /// </summary>
            public string GroupCode { get; set; } = string.Empty;


        }
    }
}
