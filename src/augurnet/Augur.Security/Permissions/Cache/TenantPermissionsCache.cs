using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Augur.Data.Interfaces.Providers;
using Augur.Security.Interfaces.Permissions.Cache;
using Augur.Security.Interfaces.Permissions.Providers;
using Augur.Security.Interfaces.Permissions.Repositories;
using Augur.Security.Permissions.Models;

namespace Augur.Security.Permissions.Cache
{
    public class TenantPermissionsCache : ITenantPermissionsCache
    {
        private readonly IAccessControlRepository securityRepository;
        private readonly IAugurLoggedInTenantIdProvider loggedInTenantIdProvider;
        private readonly IAugurLoggedInRoleIdProvider loggedInRoleIdProvider;

        private static readonly ConcurrentDictionary<int, RolePermissionLookup> _tenantRolePermissionLookupCache =
            new ConcurrentDictionary<int, RolePermissionLookup>();

        public TenantPermissionsCache(IAccessControlRepository securityRepository,
            IAugurLoggedInTenantIdProvider loggedInTenantIdProvider,
            IAugurLoggedInRoleIdProvider loggedInRoleIdProvider)
        {
            this.securityRepository = securityRepository;
            this.loggedInTenantIdProvider = loggedInTenantIdProvider;
            this.loggedInRoleIdProvider = loggedInRoleIdProvider;
        }

        public void BuildTenantPermissionsCache(int tenantId,
            ICollection<(int RoleId, ICollection<Permission> Permissions)> roleIdsWithPermissions)
        {
            var rolePermissionLookup = new RolePermissionLookup();

            foreach (var role in roleIdsWithPermissions)
            {
                var permissionLookup = this.BuildPermissionsLookup(role.Permissions);

                rolePermissionLookup.AddOrUpdate(role.RoleId, permissionLookup, (key, existingValue) =>
                {
                    foreach (var componentKey in permissionLookup.Keys)
                    {
                        if (!existingValue.ContainsKey(componentKey))
                        {
                            throw new InvalidOperationException();
                        }

                        existingValue[componentKey] = permissionLookup[componentKey];
                    }

                    return existingValue;
                });
            }

            _tenantRolePermissionLookupCache.AddOrUpdate(tenantId, rolePermissionLookup,
                (key, existingValue) => rolePermissionLookup);
        }

        public async Task BuildTenantPermissionsCache(int tenantId)
        {
            var roleIdsWithPermissions =
                await this.securityRepository.GetRoleIdsWithPermissionsForTenantAsync(tenantId);

            this.BuildTenantPermissionsCache(tenantId, roleIdsWithPermissions);
        }

        public async Task BuildLoggedInTenantPermissionsCache()
        {
            await this.BuildTenantPermissionsCache(this.loggedInTenantIdProvider.TenantId);
        }

        public async Task UpdateCacheForNewRoleForLoggedInTenant(int roleId, int roleType)
        {
            var rolePermissionLookup = _tenantRolePermissionLookupCache[this.loggedInTenantIdProvider.TenantId];

            var permissionLookup =
                await this.BuildPermissionsLookupForRole(this.loggedInTenantIdProvider.TenantId, roleId, roleType);

            rolePermissionLookup.AddOrUpdate(roleId, permissionLookup, (key, existingValue) => permissionLookup);
        }

        public void UpdateCacheForDeletedRoleForLoggedInTenant(int roleId)
        {
            var rolePermissionLookup = _tenantRolePermissionLookupCache[this.loggedInTenantIdProvider.TenantId];

            rolePermissionLookup.TryRemove(roleId, out _);
        }

        public async Task BuildAllTenantPermissionsCache()
        {
            var tenantIds = await this.securityRepository.GetAllTenantIdsAsync();

            foreach (var tenantId in tenantIds)
            {
                await this.BuildTenantPermissionsCache(tenantId);
            }
        }

        private async Task<PermissionLookup> BuildPermissionsLookupForRole(int tenantId, int roleId, int roleType)
        {
            var permissions =
                await this.securityRepository.GetPermissionsForTenantAndRoleAsync(tenantId, roleId, roleType);

            return this.BuildPermissionsLookup(permissions);
        }

        private PermissionLookup BuildPermissionsLookup(IEnumerable<Permission> permissions)
        {
            var permissionLookup = new PermissionLookup();

            foreach (var permission in permissions)
            {
                permissionLookup.AddOrUpdate
                (
                    permission.Component +
                    (!string.IsNullOrEmpty(permission.Activity) ? "." + permission.Activity : ""),
                    permission,
                    (key, existingValue) => permission
                );
            }

            return permissionLookup;
        }

        public PermissionLookup GetPermissionsLookupForRole(int tenantId, int roleId)
        {
            var tenantPermissions = _tenantRolePermissionLookupCache[tenantId];

            var rolePermissions = tenantPermissions[roleId];

            return rolePermissions;
        }

        public async Task UpdateCacheForRoleForLoggedInTenant(int roleId, int roleType,
            IList<string> componentActivities)
        {
            var permissionLookup = this.GetPermissionsLookupForRole(this.loggedInTenantIdProvider.TenantId, roleId);

            var permissions =
                await this.securityRepository.GetPermissionsForTenantAndRoleAsync(
                    this.loggedInTenantIdProvider.TenantId, roleId, roleType, componentActivities);

            foreach (var permission in permissions)
            {
                var componentActivity = string.IsNullOrWhiteSpace(permission.Activity)
                    ? permission.Component
                    : $"{permission.Component}.{permission.Activity}";

                if (!permissionLookup.ContainsKey(componentActivity))
                {
                    throw new InvalidOperationException();
                }

                if (!permissionLookup.TryUpdate(componentActivity, permission, permissionLookup[componentActivity]))
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}