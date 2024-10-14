using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.SISCONModels
{
    /// <summary>
    /// Detalle de la entrega
    /// </summary>
    public class DeliveryNodo
    {
        /// <summary>
        /// Identificador único SAP Database
        /// </summary>
        [Required]
        public int DocEntry { get; set; }
        /// <summary>
        /// Linea de la entrega
        /// </summary>
        [Required]
        public int LineNum { get; set; }
        /// <summary>
        /// Código del articulo
        /// </summary>
        [Required]
        public string CodigoArticulo { get; set; } = string.Empty;
        /// <summary>
        /// Serie del equipo
        /// </summary>
        public string Serie { get; set; } = string.Empty;
        /// <summary>
        /// Precio del Equipo con IVA
        /// </summary>
        public double precio { get; set; }
        /// <summary>
        /// Número del despacho
        /// </summary>
        [Required]
        public int DespachoNo { get; set; }
        /// <summary>
        /// La linea del equipo asignado en el detalle del despacho
        /// </summary>
        [Required]
        public int DespachoLinea { get; set; }
        /// <summary>
        /// Datos generales serie asignada en la entrega
        /// </summary>
        [Required]
        public int SerieEntrega{ get; set; }
        /// <summary>
        /// Datos generales serie del activo fijo
        /// </summary>
        [Required]
        public string SerieAF { get; set; } = string.Empty ;
        /// <summary>
        /// Datos generales clase del activo fijo
        /// </summary>
        [Required]
        public string ClaseAF { get; set; } = string.Empty ;
        /// <summary>
        /// Datos generales area de depreciación
        /// </summary>
        [Required]
        public string AreaDepreciacion { get; set; } = string.Empty;
        /// <summary>
        /// Datos generales grupo del activo fijo
        /// </summary>
        [Required]
        public string GrupoAF { get; set; } = string.Empty;
        /// <summary>
        /// Datos generales monto mínimo del activo fijo
        /// </summary>
        [Required]
        public string MontoMinimoAF { get; set; } = string.Empty;
        /// <summary>
        /// Datos generales clase de gasto del activo fijo
        /// </summary>
        [Required]
        public string ClaseGastoAF { get; set; } = string.Empty;
        /// <summary>
        /// Datos generales del código del empleado del activo fijo
        /// </summary>
        [Required]
        public string CodigoEmpleadoAF { get; set; } = string.Empty;
        /// <summary>
        /// Datos generales norma de reparto del activo fijo
        /// </summary>
        [Required]
        public string NormaRepartoAF { get; set; } = string.Empty;
        /// <summary>
        /// Guardado del nuevo código de activo fijo
        /// </summary>
        public string NewCodeAF { get; set; } = string.Empty;
    }
}
