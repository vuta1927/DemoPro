using System;
using MediatR;

namespace FuturisX.Messaging.Events
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }
        DateTime CreationDate { get; }
    }
}