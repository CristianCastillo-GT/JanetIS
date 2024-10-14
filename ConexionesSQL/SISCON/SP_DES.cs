using ConexionesSQL.Models;

namespace ConexionesSQL.SISCON
{
    public class SP_DES : BaseConnection
    {
        /// <summary>
        /// Cambia el estado a la boleta
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado DES_UPDATE_ACTIVOS_FIJOS(List<Parametros> ListParamsIn)
        {
            string procedimiento = "DES_UPDATE_ACTIVOS_FIJOS";
            BaseSQL.EjecutarQueryPorPASinRetorno(procedimiento, connectionSISCON, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
