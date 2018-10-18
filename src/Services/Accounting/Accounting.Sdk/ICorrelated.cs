using System;

namespace Accounting.Sdk
{
    public interface ICorrelated
    {
        Guid CorrelationId { get; set; }
    }
}