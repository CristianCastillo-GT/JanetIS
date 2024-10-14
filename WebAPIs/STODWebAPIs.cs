using System.Text;
using System.Text.Json;
using WebAPIs.Models;

namespace WebAPIs
{
    public class STODWebAPIs : BaseWebAPI
    {
        /// <summary>
        /// Autentica con el usuario y contraseña de STOD
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>Retorna un valor booleano si son correctos los datos</returns>
        public async Task<bool> AuthenticationSTOD(string username, string password)
        {
            using var client = new HttpClient();
            string BaseUrl = $"{this.urlSTOD}Portal.aspx/UserAuthentication";

            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            var json = JsonSerializer.Serialize(new { username, password });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(BaseUrl, data);

            if (response.IsSuccessStatusCode)
            {
                string resultWebApi = await response.Content.ReadAsStringAsync();
                return (JsonSerializer.Deserialize<ResultSTODAuth>(resultWebApi)?.d ?? "") == "ok";
            }
            return false;
        }
    }
}
