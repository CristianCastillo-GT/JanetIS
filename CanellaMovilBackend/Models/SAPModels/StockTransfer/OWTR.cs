
namespace CanellaMovilBackend.Models.StockTransfer
{
    /// <summary>
    /// Objeto de transferencias
    /// </summary>
    public class OWTR
    {

        //Encabezado

        /// <summary>
        /// ID de Traslado en SAP
        /// </summary>
        public string ENS_TrasladoSAPID { get; set; } = string.Empty;

        /// <summary>
        /// FromWareHouse / Bodega Proveniente
        /// </summary>
        public string? BodegaOrigenSAP { get; set; } = string.Empty;

        /// <summary>
        /// ToWareHouse / Bodega Destino
        /// </summary>
        public string? BodegaDestinoSAP { get; set; } = string.Empty;

        /// <summary>
        /// ItemCode / Codigo de Articulo
        /// </summary>
        public string? ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// SerialNumber / Chasis
        /// </summary>
        public string? Chasis { get; set; } = string.Empty;

        /// <summary>
        /// SerialNumber / Chasis
        /// </summary>
        public string? PedidoID { get; set; } = string.Empty;

        /// <summary>
        /// CodSAPBodega / Codigo de Bodega en SAP
        /// </summary>
        public string? CodSAPBodega { get; set; } = string.Empty;

        /// <summary>
        /// ErrorSAP / Error que devuelve SAP
        /// </summary>
        public string? ErrorSAP { get; set; } = string.Empty;

        /// <summary>
        /// MensajeExito / Mensaje de Exito de Traslado
        /// </summary>
        public string? MensajeExito { get; set; } = string.Empty;

        /// <summary>
        /// Lista de traslados
        /// </summary>
        public OWTR() { 
        }

    }
}
