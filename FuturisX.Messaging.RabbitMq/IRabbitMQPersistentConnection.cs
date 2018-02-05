using System;
using RabbitMQ.Client;

namespace FuturisX.Messaging.RabbitMq
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}