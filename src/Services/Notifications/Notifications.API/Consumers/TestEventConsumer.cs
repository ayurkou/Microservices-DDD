using System.Threading.Tasks;
using Accounting.Sdk;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Notifications.API.Consumers
{
    public class TestEventConsumer: IConsumer<TestEvent>
    {
        private readonly ILogger<TestEventConsumer> _logger;

        public TestEventConsumer(ILogger<TestEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<TestEvent> context)
        {
            _logger.LogWarning("Event has been delivered {@event}", context.Message);
            return Task.CompletedTask;
        }
    }
}