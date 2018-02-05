using System;
using System.Threading.Tasks;
using FuturisX.Dependency;

namespace FuturisX.Storage.EntityFrameworkCore
{
    public interface IDbContextSeed : ITransientDependency
    {
        Type ContextType { get; }
        Task SeedAsync();
    }
}