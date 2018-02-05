using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FuturisX.Application.Services;

namespace FuturisX.Notification.Firebase
{
    public interface IFirebaseService : IApplicationService
    {
        Task InsertRegistrationAsync(FirebaseRegistration firebaseRegistration);
        Task DeleteRegistrationAsync(Guid id);
        Task<List<FirebaseRegistration>> GetAllRegistrationAsync(long userId);
    }
}