﻿using System;
using FuturisX.Domain.Entities;
using FuturisX.Domain.Entities.Auditing;
using FuturisX.Timing;

namespace FuturisX.Notifications
{
    [Serializable]
    public class UserNotificationInfo : Entity<Guid>, IHasCreationTime
    {
        /// <summary>
        /// User Id.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Current state of the user notification.
        /// </summary>
        public virtual UserNotificationState State { get; set; }

        /// <summary>
        /// Notification Id.
        /// </summary>
        public virtual Guid NotificationId { get; set; }

        public DateTime CreationTime { get; set; }

        public UserNotificationInfo()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationInfo"/> class.
        /// </summary>
        /// <param name="create"></param>
        public UserNotificationInfo(Guid id)
        {
            Id = id;
            State = UserNotificationState.Unread;
            CreationTime = Clock.Now;
        }
    }
}