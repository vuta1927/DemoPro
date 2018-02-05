using System;

namespace FuturisX.Messaging.Events
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTime CreationDate { get; }
    }
}