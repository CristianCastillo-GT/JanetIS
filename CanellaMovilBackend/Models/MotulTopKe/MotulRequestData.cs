namespace CanellaMovilBackend.Models.MotulTopKe
{
    /// <summary>
    /// Modelo de datos para la petición de articulos motul sap canella rio hondo
    /// </summary>
    public class MotulRequestData
    {
        /// <summary>
        /// Modelo de datos para la petición de inventario de motul
        /// </summary>
        public class RequestGetInventarioMotul
        {
            /// <summary>
            /// clsEmpresa es la empresa que estamos consultando
            /// </summary>
            public string clsEmpresa { get; set; } = string.Empty;

        }
    }
}
