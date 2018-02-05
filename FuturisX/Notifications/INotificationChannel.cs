using System;
using System.Threading.Tasks;

namespace FuturisX.Notifications
{
    public interface INotificationChannel
    {
        bool CanHandle(Type notificationDataType);
        Task ProcessAsync(UserNotificationInfo userNotificationsInfo);
    }
}