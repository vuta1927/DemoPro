using System;
using Microsoft.Azure.ServiceBus;

namespace FuturisX.Messaging.Azure
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateModel();
    }
}