using ConexionesSQL.Models;

namespace ConexionesSQL.SAP
{
    public class SP_DELI : BaseConnection
    {
        /// <summary>
        /// Le permite al usuario cerrar la entrega
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado DELI_AcceptCloseDelivery(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_DELI_AcceptCloseDelivery";
            BaseSQL.EjecutarQueryPorPASinRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }

        /// <summary>
        ///  Le deniega al usuario cerrar la entrega
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado DELI_DenyCloseDelivery(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_DELI_DenyCloseDelivery";
            BaseSQL.EjecutarQueryPorPASinRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }

        /// <summary>
        ///  Actualiza el comentario del cierre de la entrega
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado DELI_ActualizarComentarioEntrega(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_DELI_ActualizarComentarioEntrega";
            BaseSQL.EjecutarQueryPorPASinRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }

        /// <summary>
        ///  Actualiza el comentario del cierre de la entrega
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado DELI_SelectEntrega(List<Parametros> ListParamsIn)
        {
            string procedimiento = "API_DELI_SelectEntrega";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }
    }
}
