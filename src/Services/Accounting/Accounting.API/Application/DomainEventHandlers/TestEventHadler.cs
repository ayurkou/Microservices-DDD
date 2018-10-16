using System.Threading;
using System.Threading.Tasks;
using Solera.Daytona.Services.Accounting.Domain;
using Solera.Daytona.Services.Accounting.Domain.Events;

namespace Accounting.API.Application.DomainEventHandlers
{
    public class TestDomainEventHadler : IDomainEventHandler<TestDomainEvent>
    {
        public async Task Handle(TestDomainEvent @event, CancellationToken cancellationToken)
        {
            await Task.Run(() => throw new System.NotImplementedException());
        }
    }
}
