using NLog;

namespace CanellaMovilBackend.Static.Utils
{
    /// <summary>
    /// Clase Generica para capturar errores de la aplicación
    /// </summary>
    public class LogsAPI
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Logs catalogados como información
        /// </summary>
        public static void Info(string message)
        {
            logger.Info(message.Replace(Environment.NewLine, ""));
        }

        /// <summary>
        /// Logs catalogados como excepciones
        /// </summary>
        public static void Exception(Exception ex)
        {
            logger.Error(ex, "Exception: ");
        }
    }
}
