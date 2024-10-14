using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.STODModels.Dashboard
{
    /// <summary>
    /// Clase - Se obtienen los nuevos clientes del año actual y del mes actual, por metas
    /// </summary>
    public class ClientesNuevos
    {
        /// <summary>
        /// Mes - Clientes del mes actual
        /// </summary>
        [Required]
        public int Mes {  get; set; }
        
        /// <summary>
        /// Anio - Clientes del año actual
        /// </summary>
        [Required]
        public int Anio { get; set; }

        /// <summary>
        /// MetaMensual - Meta de clientes mensual
        /// </summary>
        [Required]
        public int MetaMensual { get; set; }

        /// <summary>
        /// MetaAnual - Meta de clientes anual
        /// </summary>
        [Required]
        public int MetaAnual { get; set; }
    }
}
