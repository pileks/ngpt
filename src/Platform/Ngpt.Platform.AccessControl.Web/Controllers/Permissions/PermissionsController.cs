using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Augur.ApiSystemEvents;
using Augur.Data.Services;
using Augur.Security.Interfaces.Permissions.Cache;
using Augur.Web.Controllers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.AccessControl.Data.Interfaces.Providers;
using Ngpt.Platform.AccessControl.Web.Controllers.Permissions.Models;
using Microsoft.AspNetCore.Mvc;
using Ngpt.Platform.Data.Entities.PermissionOverrides;
using Ngpt.Platform.Data.Entities.Roles;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Services.Enums;

namespace Ngpt.Platform.AccessControl.Web.Controllers.Permissions
{
    [RequireSuperAdminLoggedIn]
    public class PermissionsController : AugurApiController
    {
        private readonly ITenantPermissionsCache tenantPermissionsCache;
        private readonly ILoggedInTenantIdProvider loggedInTenantIdProvider;
        private readonly TenantOwnedDbContext tenantOwnedDbContext;
        private readonly ITransactionManager transactionManager;
        private readonly ILoggedInRoleIdProvider loggedInRoleIdProvider;
        private readonly IHeaderClientNotifier headerClientNotifier;

        public PermissionsController(ITenantPermissionsCache tenantPermissionsCache,
            ILoggedInTenantIdProvider loggedInTenantIdProvider, TenantOwnedDbContext tenantOwnedDbContext,
            ITransactionManager transactionManager, ILoggedInRoleIdProvider loggedInRoleIdProvider, IHeaderClientNotifier headerClientNotifier)
        {
            this.tenantPermissionsCache = tenantPermissionsCache;
            this.loggedInTenantIdProvider = loggedInTenantIdProvider;
            this.tenantOwnedDbContext = tenantOwnedDbContext;
            this.transactionManager = transactionManager;
            this.loggedInRoleIdProvider = loggedInRoleIdProvider;
            this.headerClientNotifier = headerClientNotifier;
        }

        [HttpGet(nameof(GetPermissionsForEdit))]
        public IEnumerable<PermissionComponentModel> GetPermissionsForEdit(int roleId)
        {
            return this.tenantPermissionsCache.GetPermissionsLookupForRole(this.loggedInTenantIdProvider.TenantId, roleId)
                .Values
                .GroupBy(permission => permission.Component)
                .Select(g => new PermissionComponentModel
                {
                    Name = g.Key,
                    IsAllowed = g.Single(p => p.Activity == "")
                        .IsAllowed,
                    Activities = g.Where(p => p.Activity != "")
                        .Select(p => new PermissionActivityModel
                        {
                            Name = p.Activity,
                            IsAllowed = p.IsAllowed
                        })
                        .ToList()
                });
        }

        [HttpPost(nameof(UpdatePermissions))]
        public async Task<IEnumerable<PermissionComponentModel>> UpdatePermissions([FromBody]UpdatePermissionsModel model)
        {
            var componentActivities = model.Permissions
                .Select(c => $"{c.Component}.{c.Activity}")
                .ToList();

            var role = await this.tenantOwnedDbContext.Set<Role>().FindAsync(model.RoleId);

            var existingOverrides = this.tenantOwnedDbContext.Set<PermissionOverride>()
                .Where(p => componentActivities.Contains(p.Component + "." + p.Activity))
                .Where(p => p.RoleId == role.Id)
                .ToList();

            this.tenantOwnedDbContext.Set<PermissionOverride>().RemoveRange(existingOverrides);

            foreach (var permission in model.Permissions)
            {
                this.tenantOwnedDbContext.Set<PermissionOverride>().Add(new PermissionOverride
                {
                    Component = permission.Component,
                    Activity = permission.Activity,
                    IsAllowed = permission.IsAllowed,
                    RoleId = role.Id
                });
            }

            await this.transactionManager.UseTransactionAsync(async () =>
            {
                await this.tenantOwnedDbContext.SaveChangesAsync();

                await this.tenantPermissionsCache.UpdateCacheForRoleForLoggedInTenant(role.Id, (int)role.Type, componentActivities);
            });

            if (role.Id == this.loggedInRoleIdProvider.RoleId)
            {
                this.headerClientNotifier.Notify(this.Response, ApiSystemEvent.LoggedInUserChanged);
            }
            
            return this.GetPermissionsForEdit(role.Id);
        }
    }
}
