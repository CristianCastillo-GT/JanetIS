namespace CanellaMovilBackend.Models.SAPModels.Reports
{
    /// <summary>
    /// Modelo para la tarjeta de equipos
    /// </summary>
    public class ListadoTarjetaEquipos
    {
        /// <summary>
        /// Identificador unico de la tarjeta de equipo
        /// </summary>
        public int insID { get; set; }
        /// <summary>
        /// Número del artículo
        /// </summary>
        public string itemCode { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del artículo
        /// </summary>
        public string itemName { get; set; } = string.Empty;

        /// <summary>
        /// Código socio de negocios
        /// </summary>
        public string customer { get; set; } = string.Empty;
    }
}
