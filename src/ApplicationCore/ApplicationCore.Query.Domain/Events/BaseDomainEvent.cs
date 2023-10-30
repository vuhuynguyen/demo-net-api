using EnsureThat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Events
{
    public class BaseDomainEvent : INotification
    {
        /// <summary>
        /// The source that triggers this event.
        /// Used for tracing.
        /// </summary>
        public string Source { get; }

        public BaseDomainEvent(string source)
        {
            Ensure.That(source, nameof(source)).IsNotNullOrEmpty();

            Source = source;
        }
    }
}
