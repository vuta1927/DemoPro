using System;
using FuturisX.Messaging.Events;
using FuturisX.Timing;

namespace FuturisX.Messaging
{
    /// <summary>
    /// This type of events can be used to notify for an exception.
    /// </summary>
    public class HandleEventException : Event
    {
        /// <summary>
        /// Exception object
        /// </summary>
        public Exception Exception { get; private set; }
        
        public HandleEventException(Exception exception)
        {
            Exception = exception;
        }
    }
}