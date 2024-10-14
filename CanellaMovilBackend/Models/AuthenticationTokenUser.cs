using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models
{
    /// <summary>
    /// Modelo de datos del token
    /// </summary>
    public class AuthenticationTokenUser
    {
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        [Required]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// ID del usuario
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Correo electronico del usuario
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// El rol del usuario
        /// </summary>
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
