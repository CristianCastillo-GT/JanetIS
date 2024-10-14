using System.ComponentModel.DataAnnotations;


namespace CanellaMovilBackend.Models.SAPModels.TeamCard
{
    /// <summary>
    /// Creación de una tarjeta de equipo
    /// </summary>
    public class CreateTC
    {
        /// <summary>
        /// manufSN - No. Serie Fabricante
        /// </summary>
        public string manufSN { get; set; } = string.Empty;

        /// <summary>
        /// internalSN - No. Serie/Chasis
        /// </summary>
        public string internalSN { get; set; } = string.Empty;

        /// <summary>
        /// itemCode - Número de artículo
        /// </summary>
        public string itemCode { get; set; } = string.Empty;

        /// <summary>
        /// customer - Código socio de negocios
        /// </summary>
        public string customer { get; set; } = string.Empty;

        /// <summary>
        /// contactCod - Código Persona de contacto
        /// </summary>
        public int contactCod { get; set; }


    }
}
