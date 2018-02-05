﻿using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace FuturisX.Storage.EntityFrameworkCore
{
    public interface IDbContextResolver
    {
        TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection)
            where TDbContext : DbContext;
    }
}