using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Solera.Daytona.Services.Accounting.Sdk;

namespace Notifications.API.Consumers
{
    public class TestEventConsumer: IConsumer<NotificationPrefsChangedEvent>
    {
        private readonly ILogger<TestEventConsumer> _logger;

        public TestEventConsumer(ILogger<TestEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<NotificationPrefsChangedEvent> context)
        {
            _logger.LogWarning("Event has been delivered {@event}", context.Message);
            return Task.CompletedTask;
        }
    }
}