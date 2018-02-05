using System;
using System.Collections.Generic;

namespace FuturisX.Notification.Firebase
{
    public class DeviceSubscriptionExpiredException : DeviceSubscriptonExpiredException
    {
        public DeviceSubscriptionExpiredException(FirebaseNotificationData notification)
            : base(notification)
        {
        }
    }

    [Obsolete("Do not use this class directly, it has a typo in it, instead use DeviceSubscriptionExpiredException")]
    public class DeviceSubscriptonExpiredException : Exception
    {
        public DeviceSubscriptonExpiredException(FirebaseNotificationData notification)
            : base("Device Subscription has Expired")
        {
            Notification = notification;
            ExpiredAt = DateTime.UtcNow;
        }

        public FirebaseNotificationData Notification { get; private set; }
        public string OldSubscriptionId { get; set; }
        public string NewSubscriptionId { get; set; }
        public DateTime ExpiredAt { get; set; }
    }

    public class FirebaseNotificationException : Exception
    {
        public FirebaseNotificationException(FirebaseNotificationData notification, string mesage)
            : base(mesage)
        {
            Notification = notification;
        }

        public FirebaseNotificationException(FirebaseNotificationData notification, string mesage, string description)
            : base(mesage)
        {
            Notification = notification;
            Description = description;
        }

        public FirebaseNotificationException(FirebaseNotificationData notification, string mesage, Exception innerException)
            : base(mesage, innerException)
        {
            Notification = notification;
        }

        public FirebaseNotificationData Notification { get; private set; }
        public string Description { get; private set; }
    }

    public class FirebaseMulticastResultException : Exception
    {
        public FirebaseMulticastResultException()
            : base("One or more Registration Id's failed in the multicast notification")
        {
            Succeeded = new List<FirebaseNotificationData>();
            Failed = new Dictionary<FirebaseNotificationData, Exception>();
        }

        public List<FirebaseNotificationData> Succeeded { get; set; }

        public Dictionary<FirebaseNotificationData, Exception> Failed { get; set; }
    }

    public class RetryAfterException : Exception
    {
        public RetryAfterException(FirebaseNotificationData notification, string message, DateTime retryAfterUtc)
            : base(message)
        {
            Notification = notification;
            RetryAfterUtc = retryAfterUtc;
        }

        public RetryAfterException(FirebaseNotificationData notification, string message, DateTime retryAfterUtc,
            Exception innerException)
            : base(message, innerException)
        {
            Notification = notification;
            RetryAfterUtc = retryAfterUtc;
        }

        public FirebaseNotificationData Notification { get; private set; }
        public DateTime RetryAfterUtc { get; set; }
    }
}