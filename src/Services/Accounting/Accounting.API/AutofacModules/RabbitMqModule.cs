using System;
using Autofac;
using MassTransit;
using Newtonsoft.Json;

namespace accounting.api.AutofacModules
{
    public class RabbitMqModule: Module
    {
        private readonly RabbitMqConfig _config;

        public RabbitMqModule(RabbitMqConfig config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(_config.Host), h =>
                    {
                        h.Username(_config.Username);
                        h.Password(_config.Password);
                    });

                    cfg.ConfigureJsonSerializer(s =>
                    {
                        s.DefaultValueHandling = DefaultValueHandling.Include;
                        return s;
                    });
                    
                    cfg.ConfigureJsonDeserializer(s =>
                    {
                        s.DefaultValueHandling = DefaultValueHandling.Populate;
                        return s;
                    });

                    cfg.ReceiveEndpoint(host, _config.Queue, ec => ec.LoadFrom(context));
                });

                return bus;
            }).SingleInstance().As<IBus>().As<IBusControl>();
        }
    }
}