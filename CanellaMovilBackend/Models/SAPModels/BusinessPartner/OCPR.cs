using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.BusinessPartner
{
    /// <summary>
    /// Modelo para los contactos de los Socio de Negocios SAP
    /// </summary>
    public class OCPR
    {
        /// <summary>
        /// CntctCode - Codigo del Contacto
        /// </summary>
        public string CntctCode { get; set; } = string.Empty;

        /// <summary>
        /// CardCode - Codigo del Cliente
        /// </summary>
        public string CardCode { get; set; } = string.Empty;

        /// <summary>
        /// Contact ID - Codigo del Contacto
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// First Name - Primer Nombre
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last Name - Apellido
        /// </summary>
        public string LasName { get; set; } = string.Empty;

        /// <summary>
        /// Title - Puesto
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Telphone 1 - Telefono 1
        /// </summary>
        public string Tel1 { get; set; } = string.Empty;

        /// <summary>
        /// Telphone 2 - Telefono 2
        /// </summary>
        public string Tel2 { get; set; } = string.Empty;

        /// <summary>
        /// Mobile Phone - Celular
        /// </summary>
        public string Cellolar { get; set; } = string.Empty;

        /// <summary>
        /// E-Mail - Correo Electronico
        /// </summary>
        public string E_MailL { get; set; } = string.Empty;


    }
}