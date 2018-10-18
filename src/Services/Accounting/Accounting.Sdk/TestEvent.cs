using System;

namespace Accounting.Sdk
{
    public class TestEvent: ICorrelated
    {
        public string Title { get; set; }
        public Guid CorrelationId { get; set; }
    }
}