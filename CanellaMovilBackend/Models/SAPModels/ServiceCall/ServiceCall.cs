using SAPbobsCOM;
using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SAPModels.ServiceCall
{
    /// <summary>
    /// Llamada de Servicio
    /// </summary>
    public class ServiceCall
    {
        /// <summary>
        /// Series del taller
        /// </summary>
        [Required]
        public int Series { get; set; }

        /// <summary>
        /// Código del cliente en Socio de Negocio
        /// </summary>
        [Required]
        public string CustomerCode { get; set; } = string.Empty;

        /// <summary>
        /// Codigo de articulo
        /// </summary>
        [Required]
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// Serie del artículo
        /// </summary>
        [Required]
        public string InternalSerialNum { get; set; } = string.Empty;

        /// <summary>
        /// Serie del artículo
        /// </summary>
        [Required]
        public string ManufacturerSerialNum { get; set; } = string.Empty;

        /// <summary>
        /// Asunto
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de problema
        /// </summary>
        [Required]
        public int ProblemType { get; set; }

        /// <summary>
        /// Tipo de llamada
        /// </summary>
        [Required]
        public int CallType { get; set; }

        /// <summary>
        /// Usuario de SAP asignado
        /// </summary>
        [Required]
        public int AssigneeCode { get; set; }

        /// <summary>
        /// Origen de la llamada de servicio de donde proviene
        /// </summary>
        [Required]
        public int Origin { get; set; }

        /// <summary>
        /// Tipo de prioridad (Baja=0, Medio=1, Alto=2)
        /// </summary>
        [Required]
        public BoSvcCallPriorities Priority { get; set; }

        /// <summary>
        /// Usuario que autoriza en SAP
        /// </summary>
        public string U_AutorizaSup { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de llamada
        /// </summary>
        public string U_TipoLLamada { get; set; } = string.Empty;

        /// <summary>
        /// Si es facturable o no
        /// </summary>
        public string U_Facturable { get; set; } = string.Empty;

        /// <summary>
        /// Lugar donde se dio la atención
        /// </summary>
        public string U_LugarAtencion { get; set; } = string.Empty;

        /// <summary>
        /// Centro de costo
        /// </summary>
        public string U_CECO { get; set; } = string.Empty;
    }
}
