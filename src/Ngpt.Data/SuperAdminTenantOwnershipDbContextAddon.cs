using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using Augur.Entity.Interfaces.Base;
using Augur.Data.Interfaces.Providers;

namespace Augur.Data.DbContextAddons
{
    public class SuperAdminTenantOwnershipDbContextAddon : AugurDefaultDbContextAddon
    {
        private readonly IAugurLoggedInTenantIdProvider loggedInTenantIdProvider;

        public SuperAdminTenantOwnershipDbContextAddon(IAugurLoggedInTenantIdProvider loggedInTenantIdProvider)
        {
            this.loggedInTenantIdProvider = loggedInTenantIdProvider;
        }

        public override void SaveChangesAsync(DbContext dbContext, bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.AssignTenantIdForAddedEntities(dbContext);
        }

        public override void SaveChanges(DbContext dbContext, bool acceptAllChangesOnSuccess)
        {
            this.AssignTenantIdForAddedEntities(dbContext);
        }

        protected virtual void AssignTenantIdForAddedEntities(DbContext dbContext)
        {
            var entries = dbContext.ChangeTracker.Entries<ITenantEntity>();

            this.loggedInTenantIdProvider.RequireTenantLoggedIn();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        AssignTenantOwnership(entry.Entity);
                        break;
                    case EntityState.Modified:
                        break;
                    case EntityState.Deleted:
                        break;
                }
            }
        }

        private void AssignTenantOwnership(ITenantEntity entity)
        {
            entity.TenantId = this.loggedInTenantIdProvider.TenantId;
        }
    }
}