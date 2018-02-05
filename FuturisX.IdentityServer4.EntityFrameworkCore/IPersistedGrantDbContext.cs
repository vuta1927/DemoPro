using Microsoft.EntityFrameworkCore;

namespace FuturisX.IdentityServer4.EntityFrameworkCore
{
    public interface IPersistedGrantDbContext
    {
        DbSet<PersistedGrantEntity> PersistedGrants { get; set; }
    }
}
