using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using Augur.Entity.Interfaces.Base;
using Augur.Data.Interfaces.Providers;
using System.Linq;

namespace Augur.Data.DbContextAddons
{
    public class AugurChangeTrackingDbContextAddon : AugurDefaultDbContextAddon
    {
        protected readonly IAugurLoggedInUserIdProvider loggedInUserIdProvider;

        public AugurChangeTrackingDbContextAddon(IAugurLoggedInUserIdProvider loggedInUserIdProvider)
        {
            this.loggedInUserIdProvider = loggedInUserIdProvider;
        }

        public override void SaveChangesAsync(DbContext dbContext, bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ExecuteChangeTracking(dbContext);
        }

        public override void SaveChanges(DbContext dbContext, bool acceptAllChangesOnSuccess)
        {
            this.ExecuteChangeTracking(dbContext);
        }

        protected virtual void ExecuteChangeTracking(DbContext dbContext)
        {
            var entries = dbContext.ChangeTracker.Entries<IChangeTrackable>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedById = this.loggedInUserIdProvider.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedOn = DateTime.UtcNow;
                        entry.Entity.UpdatedById = this.loggedInUserIdProvider.UserId;
                        break;
                }
            }
        }
    }
}