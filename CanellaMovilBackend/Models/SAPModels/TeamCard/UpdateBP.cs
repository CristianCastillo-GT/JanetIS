using SAPbobsCOM;
using System.ComponentModel.DataAnnotations;


namespace CanellaMovilBackend.Models.SAPModels.TeamCard
{
    /// <summary>
    /// Actualización del Socio de Negocio para una tarjeta de equipo
    /// </summary>
    public class UpdateBP
    {

        /// <summary>
        /// insID - Código Tarjeta de Equipo
        /// </summary>
        [Required]
        public int InsID { get; set; }

        /// <summary>
        /// customer - Código socio de negocios
        /// </summary>
        [Required]
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// Estado de la tarjeta de equipo
        /// 0 = Activo
        /// 1 = Devuelto
        /// 2 = Cancelado
        /// 3 = Concedido en préstamo
        /// 4 = En laboratorio de reparación
        /// </summary>
        [Required]
        public BoSerialNumberStatus StatusOfSerialNumber { get; set; }
    }
}
