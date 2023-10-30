using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.SeedWork
{
    public abstract class Entity
    {
        /// <summary>
        /// Non-clustered PK
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Clustered identity column
        /// </summary>
        public int IdentityKey { get; private set; }

        [JsonIgnore]
        public bool IsTransient => IdentityKey == 0;

        private readonly List<INotification> _domainEvents = new();

        [JsonIgnore]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;

        public void AddDomainEvent(INotification @event)
        {
            _domainEvents.Add(@event);
        }

        public void RemoveDomainEvent(INotification @event)
        {
            _domainEvents.Remove(@event);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
