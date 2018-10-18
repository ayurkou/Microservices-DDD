using System.Reflection;
using Autofac;
using MassTransit;

namespace Notifications.API.Consumers
{
    public class ConsumersModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterConsumers(typeof(ConsumersModule).GetTypeInfo().Assembly).AsSelf()
                .AsImplementedInterfaces();
        }
    }
}