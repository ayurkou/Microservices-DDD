using System.Threading;
using System.Threading.Tasks;
using Solera.Daytona.Services.Accounting.Domain;
using Solera.Daytona.Services.Accounting.Domain.Events;

namespace Accounting.API.Application.DomainEventHandlers
{
    public class TestDomainEventHadler : IDomainEventHandler<TestInternalDomainEvent>
    {
        public async Task Handle(TestInternalDomainEvent @event, CancellationToken cancellationToken)
        {
            await Task.Run(() => throw new System.NotImplementedException());
        }
    }
}
