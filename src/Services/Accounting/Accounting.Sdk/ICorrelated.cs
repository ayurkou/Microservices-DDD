using System;

namespace Solera.Daytona.Services.Accounting.Sdk
{
    public interface ICorrelated
    {
        Guid CorrelationId { get; set; }
    }
}