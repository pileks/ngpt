using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Augur.Data.Interfaces.DbContextAddons
{
    public interface IAugurDbContextAddOn
    {
        void SaveChangesAsync(DbContext dbContext, bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default);

        void SaveChanges(DbContext dbContext, bool acceptAllChangesOnSuccess);

        IEntityTypeConfiguration<TEntity> ApplyConfiguration<TEntity>(DbContext dbContext, ModelBuilder modelBuilder,
            IEntityTypeConfiguration<TEntity> entityTypeConfiguration)
            where TEntity : class;
    }
}