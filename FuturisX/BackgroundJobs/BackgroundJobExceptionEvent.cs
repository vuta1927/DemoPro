using System;
using System.Runtime.Serialization;
using FuturisX.Messaging;
using FuturisX.Messaging.Events;
using FuturisX.Timing;
using JetBrains.Annotations;

namespace FuturisX.BackgroundJobs
{
    [Serializable]
    public class BackgroundJobExceptionEvent : AppException, IEvent
    {
        public Guid Id => Guid.NewGuid();
        public DateTime CreationDate { get; }

        [CanBeNull]
        public BackgroundJobInfo BackgroundJob { get; set; }

        [CanBeNull]
        public object JobObject { get; set; }

        public Exception Exception { get; set; }

        public BackgroundJobExceptionEvent()
        {
            CreationDate = Clock.Now;
        }

        public BackgroundJobExceptionEvent(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public BackgroundJobExceptionEvent(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}