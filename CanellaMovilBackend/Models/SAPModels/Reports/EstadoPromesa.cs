namespace CanellaMovilBackend.Models.SAPModels.Reports
{
    /// <summary>
    ///  Modelos de la tabla para cambio de estado en la Promesa
    /// </summary>
    public class EstadoPromesa
    {
        /// <summary>
        /// CardCode - Estado de respuesta
        /// </summary>
        public string resp_status { get; set; } = string.Empty;

        /// <summary>
        /// CardbName - Mensaje de respuesta de promesa
        /// </summary>
        public string resp_message { get; set; } = string.Empty;

        
    }
}
