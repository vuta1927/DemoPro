using System;
using FuturisX.Timing;

namespace FuturisX.Messaging.Events
{
    public class DomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime CreationDate { get; }

        public DomainEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = Clock.Now;
        }
    }
}