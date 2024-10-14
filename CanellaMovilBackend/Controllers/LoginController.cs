using CanellaMovilBackend.Models;
using ConexionesSQL;
using ConexionesSQL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIs;

namespace CanellaMovilBackend.Controllers
{
    /// <summary>
    /// Controlador para el login
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration; 

        /// <summary>
        /// Constructor Login Controller
        /// </summary>
        /// <param name="configuration"></param>
        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Inició de sesión del sistema
        /// </summary>
        /// <param name="authenticationUser">Credenciales del usuario de STOD</param>
        /// <returns>Token de seguridad</returns>
        /// <response code="200">Inició de sesión exitoso</response>
        /// <response code="404">Usuario no encontrado</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(Login), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Authentication(AuthenticationUser authenticationUser)
        {
            try
            {
                SP_LOGI logi = new();
                var parameters = new List<Parametros>()
                {
                    new("usuario", authenticationUser.Username)
                };

                var resultado = logi.LOGI_ObtenerCredenciales(parameters);

                if (resultado.Datos.Rows.Count == 0)
                {
                    return NotFound(new MessageAPI()
                    {
                        Result = "Fail",
                        Message = "El usuario no existe en STOD favor de validar"
                    });
                }

                var stodWebAPI = new STODWebAPIs();

                if (!(await stodWebAPI.AuthenticationSTOD(authenticationUser.Username, authenticationUser.Password)))
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "Contraseña incorrecta" });
                }
                DataRow? dataRow = resultado.Datos?.Rows[0] ?? null;

                AuthenticationTokenUser authenticationToken = new()
                {
                    Username = dataRow?["LoweredUserName"]?.ToString() ?? "",
                    UserId = dataRow?["UserId"]?.ToString() ?? "",
                    Email = dataRow?["Email"]?.ToString() ?? "",
                    Name = dataRow?["NombreCompleto"]?.ToString() ?? "",
                    Role = dataRow?["RoleUser"]?.ToString() ?? ""
                };
                string jwt = GenerateToken(authenticationToken);

                if (Request.Headers.ContainsKey("STODNET") && Request.Headers.TryGetValue("STODNET", out StringValues value) && value.ToString() == "true")
                {
                    return Ok(new Login
                    {
                        Token = jwt,
                        Username = authenticationToken.Username,
                        Email = authenticationToken.Email,
                        Name = authenticationToken.Name,
                        Avatar = ""
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Token = jwt
                    });
                }
                
            }
            catch(Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = ex.Message });
            }
        }

        /// <summary>
        /// Generador de token de autenticación
        /// </summary>
        /// <param name="authenticationToken"></param>
        /// <returns>Retorna el token de autorización</returns>
        protected string GenerateToken(AuthenticationTokenUser authenticationToken)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, authenticationToken.Username),
                new Claim(ClaimTypes.Email, authenticationToken.Email),
                new Claim(ClaimTypes.NameIdentifier, authenticationToken.UserId),
                new Claim(ClaimTypes.GivenName, authenticationToken.Name),
                new Claim(ClaimTypes.Role, authenticationToken.Role)
            };
            string jwtKey = configuration.GetSection("JWT:Key").Value??"";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
