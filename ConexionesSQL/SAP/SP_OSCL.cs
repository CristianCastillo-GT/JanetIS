using ConexionesSQL.Models;

namespace ConexionesSQL.SAP
{
    public class SP_OSCL : BaseConnection
    {
        /// <summary>
        /// Busca las llamadas de servicio en SAP
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado OSCL_SearchServiceCall(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_OSCL_SearchServiceCall";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSAP, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
