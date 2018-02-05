using System;
using System.Collections.Generic;
using FuturisX.Domain.Entities;

namespace FuturisX.Storage.EntityFrameworkCore
{
    public interface IDbContextEntityFinder
    {
        IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType);
    }
}