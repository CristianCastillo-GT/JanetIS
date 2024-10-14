namespace CanellaMovilBackend.Models.PaginasWebModels
{
    /// <summary>
    /// Modelo de datos para la petición de repuestos maquinaria sap canella y maquipos
    /// </summary>
    public class MRequestData
    {
        /// <summary>
        /// Modelo de datos para la petición de inventario de maquinaria
        /// </summary>
        public class RequestGetInventarioMaquinaria
        {
            /// <summary>
            /// AddId es el nit del cliente
            /// </summary>
            public string AddId { get; set; } = string.Empty;
            /// <summary>
            /// clsEmpresa es la empresa que estamos consultando
            /// </summary>
            public string clsEmpresa { get; set; } = string.Empty;
            /// <summary>
            /// PlataformaConsumo es el codigo de la plataforma que esta consumiendo
            /// </summary>
            public string PlataformaConsumo { get; set; } = string.Empty;

        }

        /// <summary>
        /// Modelo de datos para la petición de clientes con su facturacion de maquinaria
        /// </summary>
        public class RequestGetClientesFacturacionMaquinaria
        {
            /// <summary>
            /// AddId es el nit del cliente
            /// </summary>
            public string AddId { get; set; } = string.Empty;
            /// <summary>
            /// clsEmpresa es la empresa que estamos consultando
            /// </summary>
            public string clsEmpresa { get; set; } = string.Empty;
           

        }
    }
}
