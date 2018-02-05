using Microsoft.EntityFrameworkCore;

namespace FuturisX.Storage.EntityFrameworkCore.Repositories
{
    public interface IRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}