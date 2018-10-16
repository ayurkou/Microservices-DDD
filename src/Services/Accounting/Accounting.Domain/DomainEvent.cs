using System;
using System.Collections.Generic;

namespace Solera.Daytona.Services.Accounting.Domain
{
    public abstract class DomainEvent : MediatR.INotification
    {
        /// <summary>
        /// Domain Event Type
        /// </summary>
        public string Type { get { return this.GetType().Name; } }

        /// <summary>
        /// Created Event Date
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Event Data
        /// </summary>
        public Dictionary<string, object> Args { get; private set; }

        /// <summary>
        /// Event Correlation GUID
        /// </summary>
        public string CorrelationID { get; set; }

        public DomainEvent()
        {
            this.Created = DateTime.Now;
            this.Args = new Dictionary<string, object>();
        }
    }
}
