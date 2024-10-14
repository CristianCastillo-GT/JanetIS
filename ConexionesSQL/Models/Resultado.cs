using System.Data;

namespace ConexionesSQL.Models
{
    public class Resultado
    {
        public DataTable Datos { get; set; } = new DataTable();
        public int MensajeTipo { get; set; }
        public string MensajeDescripcion { get; set; } = string.Empty;
    }
}
