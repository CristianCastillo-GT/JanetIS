namespace CanellaMovilBackend.Models.SISCONModels
{
    /// <summary>
    /// Detalle de la entrega
    /// </summary>
    public class ListDelivery
    {
        /// <summary>
        /// DocEntry de la entrega
        /// </summary>
        public int Entrega { get; set; }
        /// <summary>
        /// Cadena de articulos creados en los activos fijos
        /// </summary>
        public string CadenaArticulo { get; set; } = string.Empty;
    }
}
