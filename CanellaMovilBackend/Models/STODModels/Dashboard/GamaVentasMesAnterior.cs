using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.STODModels.Dashboard
{
    /// <summary>
    /// Clase para obtener las ventas mensuales anterior al mes actual por marca
    /// </summary>
    public class GamaVentasMesAnterior
    {
        /// <summary>
        /// TotalGama - Cantidad vendida por marca
        /// </summary>
        [Required]
        public decimal TotalGama { get; set; }

        /// <summary>
        /// Litros - Cantidad vendida por litros
        /// </summary>
        [Required]
        public decimal Litros { get; set; }

        /// <summary>
        /// Unidades - Cantidad vendida por unidades
        /// </summary>
        [Required]
        public decimal Unidades { get; set; }
    }
}
