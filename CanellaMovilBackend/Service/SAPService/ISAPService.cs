using CanellaMovilBackend.Models.CQMModels;
using SAPbobsCOM;

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
        Company SAPB1();

        /// <summary>
        /// Desconecta de SAP
        /// </summary>
        /// <param name="company"></param>
        void SAPB1_DISCONNECT(Company company);
    }
}
