using SAPbobsCOM;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.CQMModels
{
    /// <summary>
    /// Modelo de devolución de la conexión a SAP
    /// </summary>
    public class CompanyConnection
    {
        /// <summary>
        /// Nombre de la compañia conectada
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// indica si la conexión sigue activa
        /// </summary>
        [Required]
        public Boolean Connected { get; set; }
        /// <summary>
        /// obtiene detalles de la conexión
        /// </summary>
        [Required]
        public Company Company { get; set; } = new Company();

    }
}
