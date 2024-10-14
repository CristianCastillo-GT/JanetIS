using CanellaMovilBackend.Models.SAPModels;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.CQMModels
{
    /// <summary>
    /// Listado de modificación masiva
    /// </summary>
    public class ListOCRDMassive
    {
        /// <summary>
        /// Comentario del cierre de la entrega
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Listado de entregas abiertas
        /// </summary>
        public List<OCRDMassive> ListOCRD { get; set; } = [];
    }

    /// <summary>
    /// Modelo de Socio de Negocios SAP
    /// </summary>
    public class OCRDMassive
    {
        /// <summary>
        /// Code / Codigo de Cliente
        /// </summary>
        public string CardCode { get; set; } = string.Empty;

        /// <summary>
        /// Name / Nombre o Razón Social
        /// </summary>
        [Required]
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string AddId { get; set; } = string.Empty;
        
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string VatIdUnCmp { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Phone1 { get; set; } = string.Empty;

        /// <summary>
        /// Tel 2 / Télefono 2
        /// </summary>
        [Required]
        public string Phone2 { get; set; } = string.Empty;

        /// <summary>
        /// FAX
        /// </summary>
        [Required]
        public string Fax { get; set; } = string.Empty;

        /// <summary>
        /// Mobile Phone / Télefono Móvil
        /// </summary>
        [Required]
        public string Cellular { get; set; } = string.Empty;

        /// <summary>
        /// E-Mail / Correo electrónico
        /// </summary>
        [Required]
        public string E_Mail { get; set; } = string.Empty;

    }
}
