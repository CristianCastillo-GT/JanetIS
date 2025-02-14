namespace CanellaMovilBackend.Models
{
    /// <summary>
    /// Modelo de error en el sistema
    /// </summary>
    public class MessageAPI
    {

        /// <summary>
        /// Resultado
        /// </summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// Mensaje
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Codigo nuevo hace referencia al DocEntry o Id unico
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Devuelve segunda referencia unica del documento hace referencia al DocNum u otro Id unico necesario
        /// </summary>
        public string CodeNum { get; set; } = string.Empty;
        /// <summary>
        /// Lista
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Numero de registro para la creación de activo fijo OITMAF
        /// </summary>
        public int NoRegistro { get; set; }

    }
}
