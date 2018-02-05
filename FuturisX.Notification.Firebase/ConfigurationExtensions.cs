using System;
using FuturisX.Configuration;
using FuturisX.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace FuturisX.Notification.Firebase
{
    public static class ConfigurationExtensions
    {
        public static void UseFirebase(this INotificationConfiguration configuration, Action<FirebaseNotificationConfiguration> configureAction)
        {
            var firebaseConfiguration = new FirebaseNotificationConfiguration();
            configureAction(firebaseConfiguration);

            configuration.Configure.Services.AddTransient<INotificationChannel, FirebaseNotificationChannel>();
            configuration.Configure.Services.AddSingleton(firebaseConfiguration);
        }
    }
}
