using Microsoft.Extensions.Configuration;

namespace Accounting.Api
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