using ConexionesSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionesSQL.STOD.ENS
{
    public class ENS : BaseConnection
    {
        /// <summary>
        /// Obtiene los traslados pendientes de Ensamble
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado API_ENS_GetTrasladosPendientesSAP(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_ENS_GetTrasladosPendientesSAP";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }

        /// <summary>
        /// Actualiza Error en la tabla traslados de Ensamble en STOD
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado API_ENS_SetErrorSAP(List<Parametros> ListParamsIn) 
        {
            string procedimiento = "API_ENS_SetErrorSAP";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out  Resultado res);
            return res;
        }

        /// <summary>
        /// Actualiza el DocEntry en la tabla traslados de Ensamble en STOD
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado API_ENS_SetDocEntrySAP(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_ENS_SetDocEntrySAP";
            BaseSQL.EjecutarQueryPorPASinRetorno(procedimiento,connectionSTOD, ListParamsIn,out Resultado res);
            return res;
        }

        /// <summary>
        /// Actualiza campos de usuario
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado API_ENS_ActualizarEnsambleSAP(List<Parametros> ListParamsIn) 
        {
            string procedimiento = "ENS_ActualizarEnsambleSAP";
            BaseSQL.EjecutarQueryPorPASinRetorno(procedimiento,connectionSAP,ListParamsIn,out Resultado res);
            return res;
        }

    }
}
