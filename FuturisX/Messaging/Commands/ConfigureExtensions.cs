using System;
using FuturisX.Configuration;
using FuturisX.Helpers.Exception;

namespace FuturisX.Messaging.Commands
{
    public static partial class ConfigureExtensions
    {
        public static IConfigure Message(this IConfigure configure, Action<IMessageConfiguration> configAction)
        {
            Throw.IfArgumentNull(configure, nameof(configure));
            IMessageConfiguration messageConfiguration = new MessageConfiguration(configure);
            configAction(messageConfiguration);
//            configure.RegisterServices(s => s.AddSingleton(messageConfiguration));
            return configure;
        }
    }
}