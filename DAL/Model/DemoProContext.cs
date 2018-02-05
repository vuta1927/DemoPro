using FuturisX.Data.Uow;
using FuturisX.IdentityServer4;
using FuturisX.IdentityServer4.EntityFrameworkCore;
using FuturisX.Messaging;
using FuturisX.Messaging.Events;
using FuturisX.Storage.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DAL.Model;
namespace DAL.Model
{
    public class DemoProContext: DataContextBase<DemoProContext>, IPersistedGrantDbContext
    {
        public DemoProContext(DbContextOptions<DemoProContext> options, ICurrentUnitOfWorkProvider currentUnitOfWorkProvider, IMediator eventBus)
            : base(options, currentUnitOfWorkProvider, eventBus)
        {
        }
        public DbSet<PersistedGrantEntity> PersistedGrants { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
