using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using Augur.Entity.Interfaces.Base;
using Augur.Data.Interfaces.Providers;
using System.Linq;
using System.Linq.Expressions;
using Augur.Data.Extensions;

namespace Augur.Data.DbContextAddons
{
    public class AugurUserOwnershipDbContextAddon : AugurDefaultDbContextAddon
    {
        private readonly IAugurLoggedInUserIdProvider loggedInUserIdProvider;

        public AugurUserOwnershipDbContextAddon(IAugurLoggedInUserIdProvider loggedInUserIdProvider)
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
                        ValidateAndAssignUserOwnership((IAugurUserOwnedEntityWithUserId) entry.Entity);
                        break;
                    case EntityState.Modified:
                        ValidateAndAssignUserOwnership((IAugurUserOwnedEntityWithUserId) entry.Entity);
                        break;
                    case EntityState.Deleted:
                        ValidateUserOwnership((IAugurUserOwnedEntityWithUserId) entry.Entity);
                        break;
                }
            }
        }

        private void ValidateAndAssignUserOwnership(IAugurUserOwnedEntityWithUserId entity)
        {
            this.ValidateUserOwnership(entity);

            entity.UserId = this.loggedInUserIdProvider.UserId;
        }

        private void ValidateUserOwnership(IAugurUserOwnedEntityWithUserId entity)
        {
            if (entity.UserId != 0 && entity.UserId != this.loggedInUserIdProvider.UserId)
            {
                throw new InvalidOperationException(
                    $"Entity {entity.GetType().Name} has a UserId that does not match the currently logged in user's Id.");
            }
        }
    }
}