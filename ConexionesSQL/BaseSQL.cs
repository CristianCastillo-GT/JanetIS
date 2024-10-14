using ConexionesSQL.Models;
using System.Data;
using System.Data.SqlClient;

namespace ConexionesSQL
{
    public class BaseSQL
    {
        public static void EjecutarQueryPorPASinRetorno(string ProcedimientoAlmacenado, string conectionStringNombre, List<Parametros> ListParamsIn, out Resultado resultado)
        {
            resultado = new Resultado();

            try
            {
                using SqlCommand cmd = new(ProcedimientoAlmacenado);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                // Parmetros IN
                foreach (Parametros parametro in ListParamsIn)
                {
                    cmd.Parameters.AddWithValue(parametro.Nombre, parametro.Valor);
                }

                // Parmetros Out
                SqlParameter paramMensajeTipo = new("@MensajeTipo", "")
                {
                    Direction = ParameterDirection.Output,
                    DbType = DbType.Int32
                };
                cmd.Parameters.Add(paramMensajeTipo);

                SqlParameter paramMensajeDescripcion = new("@MensajeDescripcion", "")
                {
                    Direction = ParameterDirection.Output,
                    DbType = DbType.String,
                    Size = 200
                };
                cmd.Parameters.Add(paramMensajeDescripcion);

                ProcedimientoSinDatos(cmd, conectionStringNombre);
                resultado.MensajeTipo = Convert.ToInt32(cmd.Parameters["@MensajeTipo"].Value);
                resultado.MensajeDescripcion = cmd.Parameters["@MensajeDescripcion"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                resultado = new Resultado
                {
                    MensajeTipo = 0,
                    MensajeDescripcion = ex.Message
                };
            }
        }


        public static void EjecutarQueryPorPAConRetorno(string ProcedimientoAlmacenado, string conectionStringNombre, List<Parametros> ListaParametros, out Resultado resultado)
        {
            resultado = new Resultado();
            
            try
            {
                using SqlCommand cmd = new(ProcedimientoAlmacenado);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                // Parmetros IN
                foreach (Parametros parametro in ListaParametros)
                {
                    cmd.Parameters.AddWithValue(parametro.Nombre, parametro.Valor);
                }

                // Parmetros Out
                SqlParameter paramMensajeTipo = new("@MensajeTipo", "")
                {
                    Direction = ParameterDirection.Output,
                    DbType = DbType.Int32
                };
                cmd.Parameters.Add(paramMensajeTipo);

                SqlParameter paramMensajeDescripcion = new("@MensajeDescripcion", "")
                {
                    Direction = ParameterDirection.Output,
                    DbType = DbType.String,
                    Size = 200
                };
                cmd.Parameters.Add(paramMensajeDescripcion);

                DataTable dt = new("Datos");
                dt = ProcedimientoConDatos(cmd, conectionStringNombre);
                dt.TableName = "Datos";
                resultado.Datos = dt;
                resultado.MensajeTipo = Convert.ToInt32(cmd.Parameters["@MensajeTipo"].Value);
                resultado.MensajeDescripcion = cmd.Parameters["@MensajeDescripcion"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                resultado = new Resultado
                {
                    MensajeTipo = 0,
                    MensajeDescripcion = ex.Message
                };
            }
        }


        public static DataTable ProcedimientoConDatos(SqlCommand cmd, string conectionStringNombre)
        {
            DataTable dt = new("Datos");
            using (SqlConnection cnn = new(conectionStringNombre))
            {
                cmd.Connection = cnn;
                cnn.Open();
                SqlDataAdapter da = new(cmd);
                dt = new DataTable();
                da.Fill(dt);
                dt.TableName = "Datos";
            }
            return dt;
        }


        public static void ProcedimientoSinDatos(SqlCommand cmd, string conectionStringNombre)
        {
            using SqlConnection connection = new(conectionStringNombre);
            cmd.Connection = connection;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
