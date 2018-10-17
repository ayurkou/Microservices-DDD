using Microsoft.Extensions.Configuration;

namespace accounting.api
{
    public class AppConfig
    {
        public AppConfig (IConfiguration configuration)
        {
            configuration.Bind(this);
        }
        
        public string AllowedHosts { get; set; }
    }
}