using FuturisX.BackgroundJobs;
using FuturisX.Dependency;
using FuturisX.Threading;

namespace FuturisX.Notifications
{
    public class NotificationDistributerJob : BackgroundJob<NotificationDistributerJobArgs>, ITransientDependency
    {
        private readonly INotificationDistributer _notificationDistributer;

        public NotificationDistributerJob(INotificationDistributer notificationDistributer)
        {
            _notificationDistributer = notificationDistributer;
        }

        public override void Execute(NotificationDistributerJobArgs args)
        {
            AsyncHelper.RunSync(() => _notificationDistributer.DistributeAsync(args.NotificationId));
        }
    }
}