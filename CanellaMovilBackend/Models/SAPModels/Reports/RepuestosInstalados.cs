
namespace CanellaMovilBackend.Models.SAPModels.Reports
{
    /// <summary>
    ///  Modelo Consulta Repuestos Instalados por No.Serie
    /// </summary>
    public class RepuestosInstalados
    {

        /// <summary>
        /// Numero de documento
        /// </summary>
        public int DocNum { get; set; }

        /// <summary>
        /// Fecha del documento
        /// </summary>
        public DateTime DocDate { get; set; }

        /// <summary>
        /// Codigo de cliente
        /// </summary>
        public string CardCode { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// Codigo del articulo
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// Descripcion del articulo
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad
        /// </summary>
        public decimal Quantity { get; set; } 

        /// <summary>
        /// Costo Unitario
        /// </summary>
        public decimal GrossBuyPr { get; set; }

        /// <summary>
        /// Precio Unitario
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Porcentaje de Descuento
        /// </summary>
        public decimal DiscPrcnt { get; set; } 

        /// <summary>
        /// Linea total
        /// </summary>
        public decimal LineTotal { get; set; } 

   
        /// <summary>
        /// No. Contrato
        /// </summary>
        public int U_Contrato { get; set; } 

        /// <summary>
        /// Codigo de Bodega
        /// </summary>
        
        public string WhsCode { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad en Stock
        /// </summary>
        public decimal OnHand { get; set; }

        /// <summary>
        /// Indicador si está comprometido
        /// </summary>
        public decimal IsCommited { get; set; } 

        /// <summary>
        /// Funcion que indica si está disponible
        /// </summary>
        public decimal Disponible { get; set; }

    }
}
