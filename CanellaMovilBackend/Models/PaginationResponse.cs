using CanellaMovilBackend.Models.SAPModels.Inventory;

namespace CanellaMovilBackend.Models
{
    /// <summary>
    /// Objeto generico para el retorno de un objeto paginado
    /// </summary>
    public class PaginationResponse
    {
        /// <summary>
        /// Objeto
        /// </summary>
        public List<OITM> Object { get; set; } = [];

        /// <summary>
        /// Numero de paginas
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Pagina actual
        /// </summary>
        public int CurrentPage { get; set; }
    }
}
