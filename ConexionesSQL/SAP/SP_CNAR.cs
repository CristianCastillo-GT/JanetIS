using ConexionesSQL.Models;

namespace ConexionesSQL.SAP
{
    public class SP_CNAR : BaseConnection
    {
        /// <summary>
        /// Busca los precios de repuestos por Número de artículo
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado CRCO_SelectPrecioRepuesto(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_CRCS_SelectPrecioRepuesto";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSAP, ListParamsIn, out Resultado res);
            return res;
        }


        /// <summary>
        /// Busca los repuestos instalados al equipo por Número de Serie
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado CRCS_SelectRepuestoNumSerie(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_CRCS_SelectRepuestoNumSerie";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSAP, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
