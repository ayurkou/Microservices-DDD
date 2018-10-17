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
        public RabbitMqConfig BusConfig { get; set; }
    }

    public class RabbitMqConfig
    {  
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Queue { get; set; }
    }
}