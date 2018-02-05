using FuturisX.Configuration;
using FuturisX.Helpers.Exception;
using FuturisX.Messaging.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FuturisX.Messaging.RabbitMq
{
    /// <summary>
    /// Extensions for configuring RabbitMQ related communication for messaging
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Configure <see cref="IMessageConfiguration"/> using RabbitMQ
        /// </summary>
        public static IConfigure UseRabbitMQ(this IMessageConfiguration configuration, IConnectionFactory connectionFactory)
        {
            Throw.IfArgumentNull(configuration, nameof(configuration));
            Throw.IfArgumentNull(connectionFactory, nameof(connectionFactory));

            configuration.Configure.Services.AddSingleton<IRabbitMQPersistentConnection>(provider =>
                new DefaultRabbitMQPersistentConnection(connectionFactory,
                    provider.GetService<ILogger<DefaultRabbitMQPersistentConnection>>()));
            configuration.Configure.Services.Replace(ServiceDescriptor.Singleton<IEventBus, EventBusRabbitMQ>());
            return configuration.Configure;
        }
    }
}