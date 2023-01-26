using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Augur.Security.Interfaces.Permissions.Repositories;
using Augur.Security.Permissions.Models;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.Roles;
using Ngpt.Platform.Data.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using NgptSecurityEntities = Ngpt.Platform.Data.Entities;

namespace Ngpt.Platform.AccessControl.Repositories
{
    public class AccessControlRepository : IAccessControlRepository
    {
        private readonly RootDbContext rootDbContext;

        public AccessControlRepository(RootDbContext rootDbContext)
        {
            this.rootDbContext = rootDbContext;
        }

        public async Task<IList<int>> GetAllTenantIdsAsync()
        {
            return await this.rootDbContext.Set<Tenant>()
                .Select(x => x.Id)
                .ToListAsync();
        }

        public async Task<IList<Permission>> GetPermissionsForTenantAndRoleAsync(int tenantId, int roleId, int roleType)
        {
            var overridesQuery = this.rootDbContext.Set<NgptSecurityEntities.PermissionOverrides.PermissionOverride>()
                .Include(x => x.Role)
                .Where(x => x.RoleId == roleId)
                .Where(x => x.TenantId == tenantId)
                .Select(x => new
                {
                    x.Component,
                    x.Activity,
                    x.IsAllowed,
                    IsOverride = true
                });

            var permissionUnion = await this.rootDbContext.Set<NgptSecurityEntities.AccessControl.DefaultPermission>()
                .Where(x => x.RoleType == (NgptSecurityEntities.AccessControl.RoleType)roleType)
                .Select(x => new 
                {
                    x.Component,
                    x.Activity,
                    x.IsAllowed,
                    IsOverride = false
                })
                .Union(overridesQuery)
                .ToListAsync();


            return permissionUnion
                .GroupBy(x => new { x.Component, x.Activity })
                .Select(g =>
                {
                    var defaultPermission = g.Single(x => !x.IsOverride);
                    var permissionOverride = g.SingleOrDefault(x => x.IsOverride);
                    
                    var isAllowed = permissionOverride?.IsAllowed ?? defaultPermission.IsAllowed;

                    return new Permission
                    {
                        Component = g.Key.Component,
                        Activity = g.Key.Activity,
                        IsAllowed = isAllowed
                    };
                }).ToList();
        }

        public async Task<IList<Permission>> GetPermissionsForTenantAndRoleAsync(int tenantId, int roleId, int roleType, IList<string> componentActivities)
        {
            var overridesQuery = this.rootDbContext.Set<NgptSecurityEntities.PermissionOverrides.PermissionOverride>()
                .Include(x => x.Role)
                .Where(x => x.RoleId == roleId)
                .Where(x => x.TenantId == tenantId)
                .Where(x => componentActivities.Contains(x.Component + "." + x.Activity))
                .Select(x => new
                {
                    x.Component,
                    x.Activity,
                    x.IsAllowed,
                    IsOverride = true
                });

            var permissionUnion = await this.rootDbContext.Set<NgptSecurityEntities.AccessControl.DefaultPermission>()
                .Where(x => x.RoleType == (NgptSecurityEntities.AccessControl.RoleType)roleType)
                .Where(x => componentActivities.Contains(x.Component + "." + x.Activity))
                .Select(x => new
                {
                    x.Component,
                    x.Activity,
                    x.IsAllowed,
                    IsOverride = false
                })
                .Union(overridesQuery)
                .ToListAsync();


            return permissionUnion
                .GroupBy(x => new { x.Component, x.Activity })
                .Select(g =>
                {
                    var defaultPermission = g.Single(x => !x.IsOverride);
                    var permissionOverride = g.SingleOrDefault(x => x.IsOverride);

                    var isAllowed = permissionOverride?.IsAllowed ?? defaultPermission.IsAllowed;

                    return new Permission
                    {
                        Component = g.Key.Component,
                        Activity = g.Key.Activity,
                        IsAllowed = isAllowed
                    };
                }).ToList();
        }

        public async Task<IList<(int RoleId, ICollection<Permission>)>> GetRoleIdsWithPermissionsForTenantAsync(int tenantId)
        {
            var overridesQuery = this.rootDbContext.Set<NgptSecurityEntities.PermissionOverrides.PermissionOverride>()
                .Include(x => x.Role)
                .Where(x => x.TenantId == tenantId)
                .Select(x => new
                {
                    x.Component,
                    x.Activity,
                    x.IsAllowed,

                    RoleType = x.Role.Type,
                    x.RoleId,

                    IsOverride = true
                });

            var permissionUnion = await this.rootDbContext.Set<NgptSecurityEntities.AccessControl.DefaultPermission>()
                .Select(x => new
                {
                    x.Component,
                    x.Activity,
                    x.IsAllowed,

                    x.RoleType,
                    RoleId = 0,

                    IsOverride = false
                })
                .Union(overridesQuery)
                .ToListAsync();

            var roles = await this.GetRoleIdsWithTypeForTenant(tenantId);

            return roles
                .Select(role =>
                {
                    return 
                        (
                            role.RoleId, 
                            (ICollection<Permission>)permissionUnion
                                .Where(x => (int)x.RoleType == role.RoleType)
                                .Where(x => !x.IsOverride || x.RoleId == role.RoleId)
                                .GroupBy(x => new { x.Component, x.Activity })
                                .Select(g =>
                                {
                                    var defaultPermission = g.Single(x => !x.IsOverride);
                                    var permissionOverride = g.SingleOrDefault(x => x.IsOverride);

                                    var isAllowed = permissionOverride?.IsAllowed ?? defaultPermission.IsAllowed;

                                    return new Permission
                                    {
                                        Component = g.Key.Component,
                                        Activity = g.Key.Activity,
                                        IsAllowed = isAllowed
                                    };
                                }).ToList()
                        );
                }).ToList();
        }

        private async Task<IList<(int RoleId, int RoleType)>> GetRoleIdsWithTypeForTenant(int tenantId)
        {
            return await this.rootDbContext.Set<Role>()
                .Where(x => x.TenantId == tenantId)
                .Select(x => ValueTuple.Create(x.Id, (int)x.Type))
                .ToListAsync();
        }
    }
}
