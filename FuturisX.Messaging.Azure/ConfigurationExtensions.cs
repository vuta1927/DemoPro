using System;
using FuturisX.Configuration;
using FuturisX.Helpers.Exception;
using FuturisX.Messaging.Events;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FuturisX.Messaging.Azure
{
    public static class ConfigurationExtensions
    {
        public static IConfigure UseAzure(
            this IMessageConfiguration configuration,
            ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder,
            Action<ServiceBusConfig> serviceBusConfigAction)
        {
            Throw.IfArgumentNull(configuration, nameof(configuration));
            Throw.IfArgumentNull(serviceBusConnectionStringBuilder, nameof(serviceBusConnectionStringBuilder));
            Throw.IfArgumentNull(serviceBusConfigAction, nameof(serviceBusConfigAction));

            var serviceBusConfig = new ServiceBusConfig();
            serviceBusConfigAction(serviceBusConfig);

            configuration.Configure.Services.AddSingleton(serviceBusConfig);
            configuration.Configure.Services.AddSingleton(serviceBusConnectionStringBuilder);
            configuration.Configure.Services.AddSingleton<IServiceBusPersisterConnection, DefaultServiceBusPersisterConnection>();
            configuration.Configure.Services.Replace(ServiceDescriptor.Singleton<IEventBus, EventBusServiceBus>());

            return configuration.Configure;
        }
    }
}