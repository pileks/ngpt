using Augur.Data.Interfaces.DbContextAddons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Augur.Data.DbContextAddons
{
    public abstract class AugurDefaultDbContextAddon : IAugurDbContextAddOn
    {
        public virtual void SaveChangesAsync(DbContext dbContext, bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
        }

        public virtual void SaveChanges(DbContext dbContext, bool acceptAllChangesOnSuccess)
        {
        }

        public virtual IEntityTypeConfiguration<TEntity> ApplyConfiguration<TEntity>(DbContext dbContext,
            ModelBuilder modelBuilder, IEntityTypeConfiguration<TEntity> entityTypeConfiguration)
            where TEntity : class
        {
            return entityTypeConfiguration;
        }
    }
}