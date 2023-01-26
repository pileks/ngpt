using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Augur.ApiSystemEvents;
using Augur.Security.Interfaces.Permissions.Cache;
using Augur.Web.Controllers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.Roles;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.AccessControl.Data.Interfaces.Providers;
using Ngpt.Platform.Services.Enums;

namespace Ngpt.Platform.AccessControl.Web.Controllers.Roles
{
    [RequireSuperAdminLoggedIn]
    public class RolesController : AugurEntityController<Role>
    {
        private readonly ITenantPermissionsCache tenantPermissionsCache;
        private readonly IHeaderClientNotifier headerClientNotifier;
        private readonly ILoggedInRoleIdProvider loggedInRoleIdProvider;

        public RolesController(TenantOwnedDbContext tenantOwnedDbContext, ITenantPermissionsCache tenantPermissionsCache, IHeaderClientNotifier clientSystemEventNotifier,
            ILoggedInRoleIdProvider loggedInRoleIdProvider) : base(tenantOwnedDbContext)
        {
            this.tenantPermissionsCache = tenantPermissionsCache;
            this.headerClientNotifier = clientSystemEventNotifier;
            this.loggedInRoleIdProvider = loggedInRoleIdProvider;
        }
        
        protected override async Task AfterCreate(Role dbEntity)
        {
            await this.tenantPermissionsCache.UpdateCacheForNewRoleForLoggedInTenant(dbEntity.Id, (int)dbEntity.Type);
        }

        protected override Task Update(Role entity, Role dbEntity)
        {
            dbEntity.Name = entity.Name;
            dbEntity.Type = entity.Type;
            dbEntity.Priority = entity.Priority;

            if (dbEntity.Id == this.loggedInRoleIdProvider.RoleId)
            {
                this.headerClientNotifier.Notify(this.Response, ApiSystemEvent.LoggedInUserChanged);
            }

            return Task.CompletedTask;
        }

        protected override Task AfterDelete(Role dbEntity)
        {
            this.tenantPermissionsCache.UpdateCacheForDeletedRoleForLoggedInTenant(dbEntity.Id);

            return Task.CompletedTask;
        }

        protected override Expression<Func<Role, object>> MapListResult()
        {
            return entity => new
            {
                entity.Id,
                entity.Name,
                entity.Type,
                entity.Priority
            };
        }

        protected override IQueryable<Role> ApplySearchQueryFilter(IQueryable<Role> query, string searchQuery)
        {
            return string.IsNullOrEmpty(searchQuery)
                ? query
                : query.Where(x => x.Name.Contains(searchQuery));
        }
    }
}
