using ConexionesSQL.Models;

namespace ConexionesSQL
{
    public class SP_ROLE : BaseConnection
    {
        /// <summary>
        /// Se obtiene el permiso del role
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado ROLE_Permisos(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_ROLE_Permisos";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
