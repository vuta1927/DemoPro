using FuturisX.Data.Repositories;
using FuturisX.Data.Uow;
using FuturisX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FuturisX.Storage.EntityFrameworkCore.Repositories
{
    public class EfCoreRepositoryBase<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        public EfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider, IUnitOfWorkManager unitOfWorkManager) 
            : base(dbContextProvider, unitOfWorkManager)
        {
        }
    }
}