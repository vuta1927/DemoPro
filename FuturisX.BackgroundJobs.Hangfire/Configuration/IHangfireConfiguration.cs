using Hangfire;

namespace FuturisX.BackgroundJobs.Hangfire.Configuration
{
    public interface IHangfireConfiguration
    {
        BackgroundJobServer Server { get; set; }

        IGlobalConfiguration GlobalConfiguration { get; }
    }
}