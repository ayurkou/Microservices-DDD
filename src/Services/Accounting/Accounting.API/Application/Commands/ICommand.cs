using MediatR;

namespace Accounting.API.Application.Commands
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {

    }
}
