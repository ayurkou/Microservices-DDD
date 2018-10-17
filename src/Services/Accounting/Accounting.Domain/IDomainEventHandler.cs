using System.Threading;
using System.Threading.Tasks;

namespace Solera.Daytona.Services.Accounting.Domain
{
    public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : DomainEvent
    {
        Task Handle(TDomainEvent @event, CancellationToken cancellationToken);
    }
}
