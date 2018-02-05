using Microsoft.EntityFrameworkCore;

namespace FuturisX.Storage.EntityFrameworkCore
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}