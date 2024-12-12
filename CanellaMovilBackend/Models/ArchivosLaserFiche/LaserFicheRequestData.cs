namespace CanellaMovilBackend.Models.ArchivosLaserFiche
{
    /// <summary>
    /// Modelo de datos para la petición de articulos motul sap canella rio hondo
    /// </summary>
    public class LaserFicheRequestData
    {
        /// <summary>
        /// Modelo de datos para la petición de inventario de motul
        /// </summary>
        public class RequestGetDirectoriosLaserFiche
        {
            /// <summary>
            /// clsEmpresa es la empresa que estamos consultando
            /// </summary>
            public string clsEmpresa { get; set; } = string.Empty;

        }
    }
}
