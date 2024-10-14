using Microsoft.Extensions.Configuration;

namespace WebAPIs
{
    public class BaseWebAPI
    {
        protected string urlSTOD;
        public BaseWebAPI()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();
            this.urlSTOD = configuration?.GetConnectionString("URLSTOD") ?? "";
        }
    }
}
