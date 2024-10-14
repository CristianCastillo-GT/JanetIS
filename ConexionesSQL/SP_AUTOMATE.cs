using ConexionesSQL.Models;

namespace ConexionesSQL
{
    public class SP_AUTOMATE : BaseConnection
    {
        /// <summary>
        /// Cambia el estado a la boleta
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado AUTOMATE_UpdateEstadoConfirmacionBoletas(List<Parametros> ListParamsIn)
        {
            string procedimiento = "AUTOMATE_UpdateEstadoConfirmacionBoletas";
            BaseSQL.EjecutarQueryPorPASinRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }
        /// <summary>
        /// Cambia el estado a la boleta
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado AUTOMATE__CRI_UpdateDatosConsultaRepuestos(List<Parametros> ListParamsIn)
        {
            string procedimiento = "AUTOMATE__CRI_UpdateDatosConsultaRepuestos";
            BaseSQL.EjecutarQueryPorPASinRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
