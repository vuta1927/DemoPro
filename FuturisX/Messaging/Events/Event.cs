using System;
using FuturisX.Timing;

namespace FuturisX.Messaging.Events
{
    public class Event : IEvent
    {
        public Guid Id { get; }
        public DateTime CreationDate { get; }

        public Event()
        {
            Id = Guid.NewGuid();
            CreationDate = Clock.Now;
        }
    }
}