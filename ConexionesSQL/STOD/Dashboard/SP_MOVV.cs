using ConexionesSQL.Models;

namespace ConexionesSQL.STOD.Dashboard
{
    public class SP_MOVV : BaseConnection
    {
        /// <summary>
        /// Se obtienen las ventas anuales o mensuales por gama y marca
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado MOVV_DashboardGamaMarca(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_MOVV_DashboardGamaMarca";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionUTILS, ListParamsIn, out Resultado res);
            return res;
        }

        /// <summary>
        /// Se obtienen las ventas mensuales anterior al mes actual por marca
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado MOVV_DashboardGamaMarcaMesAnterior(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_MOVV_DashboardGamaMarcaMesAnterior";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionUTILS, ListParamsIn, out Resultado res);
            return res;
        }

        /// <summary>
        /// Se obtienen los nuevos clientes del año actual y del mes actual
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado MOVV_DashboardNuevosClientes(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_MOVV_DashboardNuevosClientes";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionUTILS, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
