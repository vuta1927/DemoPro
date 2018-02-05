using System;
using FuturisX.BackgroundJobs.Hangfire.Configuration;
using FuturisX.Configuration;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace FuturisX.BackgroundJobs.Hangfire
{
    public static class ConfigurationExtensions
    {
        public static IConfigure UsingHangfire(this IBackgroundJobConfiguration configuration,
            Action<IHangfireConfiguration> hangfireAction)
        {
            configuration.IsJobExecutionEnabled = true;

            var hangfireConfiguration = new HangfireConfiguration();
            hangfireAction(hangfireConfiguration);

            var services = configuration.Configure.Services;

            services.AddSingleton<IHangfireConfiguration>(hangfireConfiguration);
            services.AddSingleton<IBackgroundJobManager, HangfireBackgroundJobManager>();

            hangfireConfiguration.GlobalConfiguration.UseActivator(new HangfireIocJobActivator(services.BuildServiceProvider()));
            GlobalJobFilters.Filters.Add(services.BuildServiceProvider().GetService<HangfireJobExceptionFilter>());
            
            return configuration.Configure;
        }
    }
}