using ConexionesSQL.Models;

namespace ConexionesSQL.STOD.Reporte_Facturas_a_Entregar_a_Cobrador
{
    public class SP_OFC : BaseConnection
    {
        /// <summary>
        ///  Se obtienen las facturas Canella para Entregar a Cobrador
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado OFC_ObtenerFacturas_Creditos(List<Parametros> ListParamsIn)
        {
            string procedimiento = "rpt_ObtenerFacturas_CREDITOS";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }

        /// <summary>
        ///  Se obtienen las facturas de VESA para Entregar a Cobrador
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado OFC_ObtenerFacturas_CreditosVesa(List<Parametros> ListParamsIn)
        {
            string procedimiento = "COVE_Rep_ObtenerFacturasEntrega";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }

        /// <summary>
        ///  Se obtienen las facturas de VESA para Entregar a Cobrador
        /// </summary>
        /// <param name="ListParamsIn">Parametros de entrada</param>
        /// <returns>Resultado</returns>
        public Resultado OFC_ObtenerFacturas_CreditosVesaMaquipos(List<Parametros> ListParamsIn)
        {
            string procedimiento = "COMA_Rep_ObtenerFacturasEntrega";
            BaseSQL.EjecutarQueryPorPAConRetorno(procedimiento, connectionSTOD, ListParamsIn, out Resultado res);
            return res;
        }
    }

}
