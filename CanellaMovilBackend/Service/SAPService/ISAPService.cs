using CanellaMovilBackend.Models.CQMModels;

namespace CanellaMovilBackend.Service.SAPService
{
    /// <summary>
    /// Interfaz JWT User
    /// </summary>
    public interface ISAPService
    {
        /// <summary>
        /// Retorna la conexión hacia SAP producción
        /// </summary>
        CompanyConnection SAPB1();
    }
}
