using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.BusinessPartner
{
    /// <summary>
    /// Modelo para las direcciones de los Socio de Negocios SAP
    /// </summary>
    public class CRD1
    {
        /// <summary>
        /// Tipo
        /// </summary>
        [Required(ErrorMessage = "El campo Address es obligatorio")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Tipo de Dirección
        /// </summary>
        [RegularExpression("^[BS]$", ErrorMessage = "El campo solo puede ser (BillTo => 'B') o ( ShipTo => 'S').")]
        public string AddressType { get; set; } = string.Empty;

        /// <summary>
        /// Dirección
        /// </summary>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Municipio
        /// </summary>
        public string County { get; set; } = string.Empty;
        /// <summary>
        /// Departamento
        /// </summary>
        public string State { get; set; } = string.Empty;
        /// <summary>
        /// Pais
        /// </summary>
        public string Country { get; set; } = string.Empty;


    }
}