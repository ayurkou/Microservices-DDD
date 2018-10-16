using Accounting.API.Application.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.API.Application.CommandHandlers
{
    interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        new Task<TResponse> Handle(TRequest command, CancellationToken cancellationToken);
    }
}
