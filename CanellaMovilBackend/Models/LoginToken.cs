using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models
{
    /// <summary>
    /// Modelo de devolución del token y datos del usuario
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Token de autorización
        /// </summary>
        [Required]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de usuario
        /// </summary>
        [Required]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Link de la foto del usuario
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
