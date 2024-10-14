using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.STODModels.Dashboard
{
    /// <summary>
    /// Clase - Ventas hechas mensuales y anuales por gama y marca
    /// </summary>
    public class GamaVentas
    {
        /// <summary>
        /// Gama - Nombre de la gama del articulo hecha en la venta
        /// </summary>
        [Required]
        public string Gama { get; set; } = string.Empty;

        /// <summary>
        /// TotalGama - Cantidad vendida de la gama ya sea mensual o anual
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

        /// <summary>
        /// Meta - Cantidad mensual o anual que debe hacer la gama.
        /// </summary>
        [Required]
        public decimal Meta { get; set; }
    }
}
