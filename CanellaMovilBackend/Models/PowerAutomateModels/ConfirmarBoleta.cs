using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.PowerAutomateModels
{
    /// <summary>
    /// Modelo de actualizacion de boleta
    /// </summary>
    public class ConfirmarBoleta
    {
        /// <summary>
        /// IdToken
        /// </summary>
        [Required]
        public string IdSTOD { get; set; } = string.Empty;
        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        public string Estado { get; set; } = string.Empty;
        /// <summary>
        /// Id del power automate
        /// </summary>
        public string ID { get; set; } = "";
    }
    /// <summary>
    /// Modelo de actualizacion de boleta
    /// </summary>
    public class ConsultarRepuestos
    {
        /// <summary>
        /// IdToken
        /// </summary>
        [Required]
        public string IdSTOD { get; set; } = string.Empty;
        /// <summary>
        /// Pieza disponible
        /// </summary>
        [Required]
        public string PiezaDisponible { get; set; } = string.Empty;
        /// <summary>
        /// Descripcion de la pieza
        /// </summary>
        public string Descripcion { get; set; } = "";
        /// <summary>
        /// Descripcion de la pieza
        /// </summary>
        public string UnidadesPorPaquete { get; set; } = "";
        /// <summary>
        /// Descripcion de la pieza
        /// </summary>
        public string ExistenciasEnFabrica { get; set; } = "";
        /// <summary>
        /// Descripcion de la pieza
        /// </summary>
        public string Precio { get; set; } = "";

    }
}
