namespace ConexionesSQL.Models
{
    public class Parametros
    {
        public Parametros(string? _Nombre = null, object? _Valor = null)
        {
            Nombre = _Nombre;
            Valor = _Valor;
        }

        public string? Nombre { get; set; }
        public object? Valor { get; set; }
    }
}
