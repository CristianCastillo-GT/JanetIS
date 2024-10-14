
namespace CanellaMovilBackend.Models.SAPModels.Reports
{
    /// <summary>
    ///  Modelo Consulta Repuestos por Código Articulo
    /// </summary>
    public class ListadoArticulos
    {


        /// <summary>
        /// Nombre del artículo
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del artículo
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Precio del artículo en Lista
        /// </summary>
        public decimal Price { get; set; } 

        /// <summary>
        /// Nombre del elemento en tipo Lista, será siempre Serv Técnico-Público
        /// </summary>
        public string ListName { get; set; } = string.Empty;


    }
}
