using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FuturisX.Data.Repositories;
using FuturisX.Data.Uow;

namespace FuturisX.Notification.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        private readonly IRepository<FirebaseRegistration, Guid> _firebaseRegistrationRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FirebaseService(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<FirebaseRegistration, Guid> firebaseRegistrationRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _firebaseRegistrationRepository = firebaseRegistrationRepository;
        }

        public async Task InsertRegistrationAsync(FirebaseRegistration firebaseRegistration)
        {
            await _unitOfWorkManager.PerformAsyncUow<Task>(() => _firebaseRegistrationRepository.InsertAsync(firebaseRegistration));
        }

        public async Task DeleteRegistrationAsync(Guid id)
        {
            await _unitOfWorkManager.PerformAsyncUow(() => _firebaseRegistrationRepository.DeleteAsync(id));
        }

        public async Task<List<FirebaseRegistration>> GetAllRegistrationAsync(long userId)
        {
            return await _firebaseRegistrationRepository.GetAllListAsync(r => r.UserId == userId);
        }
    }
}