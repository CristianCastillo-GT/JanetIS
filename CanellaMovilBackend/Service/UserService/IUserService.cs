using CanellaMovilBackend.Models;

namespace CanellaMovilBackend.Service.UserService
{
    /// <summary>
    /// Interfaz JWT User
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Desencriptar datos del token
        /// </summary>
        /// <returns>Devolución de los datos del token</returns>
        AuthenticationTokenUser GetAuthenticationToken();
    }
}
