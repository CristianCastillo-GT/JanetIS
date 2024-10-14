using ConexionesSQL.Models;

namespace ConexionesSQL
{
    public class SP_LOGI : BaseConnection
    {
        /// <summary>
        /// Valida si el usuario existe en la base de datos y nos traes los datos del perfil
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado LOGI_ObtenerCredenciales(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_LOGI_ObtenerCredenciales";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
