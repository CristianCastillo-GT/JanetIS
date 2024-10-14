using ConexionesSQL;
using ConexionesSQL.Models;

namespace CanellaMovilBackend.Models.SAPModels.TeamCard
{
    /// <summary>
    /// Procedimientos de Tarjeta de equipos
    /// </summary>
    public class SelectTC : BaseConnection
    {
        /// <summary>
        /// Busca las llamadas de servicio en SAP
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado OINS_SelectTC(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_OINS_SelectTC";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSAP, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
