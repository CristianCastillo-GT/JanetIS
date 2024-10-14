using CanellaMovilBackend.Models;
using System.Security.Claims;

namespace CanellaMovilBackend.Service.UserService
{
    /// <summary>
    /// Servicio de los datos del token
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccesor = httpContextAccessor;
        }

        /// <summary>
        /// Desencriptar datos del token
        /// </summary>
        /// <returns>Devolución de los datos del token</returns>
        public AuthenticationTokenUser GetAuthenticationToken()
        {
            if (_httpContextAccesor.HttpContext != null)
            {
                return new AuthenticationTokenUser()
                {
                    Username = _httpContextAccesor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "",
                    UserId = _httpContextAccesor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "",
                    Email = _httpContextAccesor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? "",
                    Name = _httpContextAccesor?.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName) ?? "",
                    Role = _httpContextAccesor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) ?? ""
                };
            }
            return new AuthenticationTokenUser();
        }
    }
}
