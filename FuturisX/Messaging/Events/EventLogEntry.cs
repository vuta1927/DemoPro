using System;
using FuturisX.Domain.Entities;
using Newtonsoft.Json;

namespace FuturisX.Messaging.Events
{
    public class EventLogEntry : Entity<Guid>
    {
        private EventLogEntry() { }

        public EventLogEntry(IEvent @event)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventStateEnum.NotPublished;
            TimesSend = 0;
        }

        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        public EventStateEnum State { get; set; }
        public int TimesSend { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
    }
}