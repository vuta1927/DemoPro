using System;
using FuturisX.Data.Uow;
using Microsoft.Extensions.DependencyInjection;

namespace FuturisX.Storage.EntityFrameworkCore
{
    public interface IEfGenericRepositoryRegistrar
    {
        void RegisterForDbContext(Type dbContextType, IServiceCollection services, AutoRepositoryTypesAttribute defaultAutoRepositoryTypesAttribute);
    }
}