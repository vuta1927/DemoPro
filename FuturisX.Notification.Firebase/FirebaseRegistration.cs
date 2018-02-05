using System;
using FuturisX.Domain.Entities.Auditing;

namespace FuturisX.Notification.Firebase
{
    public class FirebaseRegistration : CreationAuditedEntity<Guid>
    {
        public string RegistrationId { get; set; }
        public string DeviceId { get; set; }
        public long UserId { get; set; }
    }
}