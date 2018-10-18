using MassTransit;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notifications.Api;

namespace Notifications.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args);
            var logger = host.Services.GetService<ILogger<Program>>();
            var bus = host.Services.GetService<IBusControl>();
            var applicationLifetime = host.Services.GetService<IApplicationLifetime>();
            applicationLifetime.ApplicationStopping.Register(() => bus.Stop());
            
            using (logger.BeginScope("{app}", "Daytona_Notifications"))
            {
                bus.Start();
                host.Run();
            }
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}