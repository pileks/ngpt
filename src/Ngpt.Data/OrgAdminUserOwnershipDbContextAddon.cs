using System;
using System.Linq;
using System.Threading;
using Augur.Data.DbContextAddons;
using Augur.Entity.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;

namespace Ngpt.Data
{
    public class OrgAdminUserOwnershipDbContextAddon : AugurDefaultDbContextAddon
    {
        private readonly ILoggedInUserIdProvider loggedInUserIdProvider;

        public OrgAdminUserOwnershipDbContextAddon(ILoggedInUserIdProvider loggedInUserIdProvider)
        {
            this.loggedInUserIdProvider = loggedInUserIdProvider;
        }

        public override void SaveChangesAsync(DbContext dbContext, bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ValidateOrAssignUserIdForChangedEntities(dbContext);
        }

        public override void SaveChanges(DbContext dbContext, bool acceptAllChangesOnSuccess)
        {
            this.ValidateOrAssignUserIdForChangedEntities(dbContext);
        }

        protected virtual void ValidateOrAssignUserIdForChangedEntities(DbContext dbContext)
        {
            var entries = dbContext.ChangeTracker.Entries<IAugurUserOwnedEntity>().ToList();

            if (entries.Any(e => !(e.Entity is IAugurUserOwnedEntityWithUserId)))
            {
                throw new InvalidOperationException("User-owned entities without UserId are not supported.");
            }


            this.loggedInUserIdProvider.RequireUserLoggedIn();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        this.AssignUserOwnership((IAugurUserOwnedEntityWithUserId) entry.Entity);
                        break;
                    case EntityState.Modified:
                        break;
                    case EntityState.Deleted:
                        break;
                }
            }
        }

        private void AssignUserOwnership(IAugurUserOwnedEntityWithUserId entity)
        {
            entity.UserId = this.loggedInUserIdProvider.UserId;
        }
    }
}