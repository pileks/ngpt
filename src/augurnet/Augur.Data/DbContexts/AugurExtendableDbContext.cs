using System.Threading;
using System.Threading.Tasks;
using Augur.Data.Interfaces.DbContextAddons;
using Microsoft.EntityFrameworkCore;

namespace Augur.Data.DbContexts
{
    public abstract class AugurExtendableDbContext : DbContext
    {
        private readonly IAugurDbContextAddOn[] dbContextAddOns;

        protected AugurExtendableDbContext(DbContextOptions options, params IAugurDbContextAddOn[] dbContextAddOns)
            : base(options)
        {
            this.dbContextAddOns = dbContextAddOns;
        }

        protected void ApplyConfiguration<TEntity>(ModelBuilder modelBuilder,
            IEntityTypeConfiguration<TEntity> entityTypeConfiguration)
            where TEntity : class
        {
            if (this.dbContextAddOns != null)
            {
                foreach (var dbContextAddon in this.dbContextAddOns)
                {
                    entityTypeConfiguration =
                        dbContextAddon.ApplyConfiguration(this, modelBuilder, entityTypeConfiguration);
                }
            }

            modelBuilder.ApplyConfiguration(entityTypeConfiguration);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.dbContextAddOns != null)
            {
                foreach (var dbContextAddon in this.dbContextAddOns)
                {
                    dbContextAddon.SaveChangesAsync(this, acceptAllChangesOnSuccess, cancellationToken);
                }
            }

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (this.dbContextAddOns != null)
            {
                foreach (var dbContextAddon in this.dbContextAddOns)
                {
                    dbContextAddon.SaveChangesAsync(this, acceptAllChangesOnSuccess);
                }
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}