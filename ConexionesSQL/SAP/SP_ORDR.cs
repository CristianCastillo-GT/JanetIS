using ConexionesSQL.Models;

namespace ConexionesSQL.SAP
{
    public class SP_ORDR : BaseConnection
    {
        /// <summary>
        /// Obtiene la orden de venta en SAP
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado ORDR_GETSaleOrder(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_ORDR_GETSaleOrder";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSAP, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
