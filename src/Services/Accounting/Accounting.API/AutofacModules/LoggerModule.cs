using Autofac;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Accounting.Api.AutofacModules
{
    public class LoggerModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx =>
            {
                var factory = new LoggerFactory()
                    .AddSerilog(new LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .CreateLogger());
                return factory;
            }).SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
        }
    }
}