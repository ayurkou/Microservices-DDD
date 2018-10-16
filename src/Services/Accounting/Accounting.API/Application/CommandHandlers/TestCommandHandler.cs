using Accounting.API.Application.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.API.Application.CommandHandlers
{
    public class TestCommandHandler : ICommandHandler<TestCommand, object>
    {
        public async Task<object> Handle(TestCommand command, CancellationToken cancellationToken)
        {
            return await Task.Run<object>(() => { throw new NotImplementedException(); });
        }
    }
}
