using ConexionesSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionesSQL.STOD.RECI
{
    public class RECI : BaseConnection
    {
        /// <summary>
        /// Obtiene vendedor por codigo
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado API_RECI_CodigoEmpleado(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_RECI_CodigoEmpleado";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }
        /// <summary>
        /// Obtiene el CounterRef
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado API_RECI_CounterReference(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_RECI_CounterReference";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
