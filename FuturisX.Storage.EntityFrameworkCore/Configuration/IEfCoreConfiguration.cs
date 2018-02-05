using System;
using FuturisX.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FuturisX.Storage.EntityFrameworkCore.Configuration
{
    public interface IEfCoreConfiguration : IConfigurator
    {
        void AddDbContext<TDbContext>(Action<DbContextConfiguration<TDbContext>> action)
            where TDbContext : DbContext;
    }
}