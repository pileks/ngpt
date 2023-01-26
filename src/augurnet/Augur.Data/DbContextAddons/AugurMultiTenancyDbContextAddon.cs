using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using Augur.Entity.Interfaces.Base;
using Augur.Data.Interfaces.Providers;

namespace Augur.Data.DbContextAddons
{
    public class AugurMultiTenancyDbContextAddon : AugurDefaultDbContextAddon
    {
        private readonly IAugurLoggedInTenantIdProvider loggedInTenantIdProvider;

        public AugurMultiTenancyDbContextAddon(IAugurLoggedInTenantIdProvider loggedInTenantIdProvider)
        {
            this.loggedInTenantIdProvider = loggedInTenantIdProvider;
        }

        public override void SaveChangesAsync(DbContext dbContext, bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ValidateOrAssignTenantIdForChangedEntities(dbContext);
        }

        public override void SaveChanges(DbContext dbContext, bool acceptAllChangesOnSuccess)
        {
            this.ValidateOrAssignTenantIdForChangedEntities(dbContext);
        }

        protected virtual void ValidateOrAssignTenantIdForChangedEntities(DbContext dbContext)
        {
            var entries = dbContext.ChangeTracker.Entries<ITenantEntity>();

            this.loggedInTenantIdProvider.RequireTenantLoggedIn();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        ValidateAndAssignTenantOwnership(entry.Entity);
                        break;
                    case EntityState.Modified:
                        ValidateAndAssignTenantOwnership(entry.Entity);
                        break;
                    case EntityState.Deleted:
                        ValidateTenantOwnership(entry.Entity);
                        break;
                }
            }
        }

        private void ValidateAndAssignTenantOwnership(ITenantEntity entity)
        {
            this.ValidateTenantOwnership(entity);

            entity.TenantId = this.loggedInTenantIdProvider.TenantId;
        }

        private void ValidateTenantOwnership(ITenantEntity entity)
        {
            if (entity.TenantId != 0 && entity.TenantId != this.loggedInTenantIdProvider.TenantId)
            {
                throw new InvalidOperationException(
                    $"Entity {entity.GetType().Name} has a TenantId that does not match the currently logged in tenant's Id.");
            }
        }
    }
}