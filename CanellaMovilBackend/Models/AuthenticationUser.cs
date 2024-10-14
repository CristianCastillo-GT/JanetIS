using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models
{
    /// <summary>
    /// Modelo de autenticación
    /// </summary>
    public class AuthenticationUser
    {
        /// <summary>
        /// Usuario de STOD
        /// </summary>
        [Required]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario en STOD
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
