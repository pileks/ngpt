using System.Collections.Generic;
using System.Threading.Tasks;
using Augur.Security.Permissions.Cache;

namespace Augur.Security.Interfaces.Permissions.Cache
{
    public interface ITenantPermissionsCache
    {
        Task BuildTenantPermissionsCache(int tenantId);
        Task BuildAllTenantPermissionsCache();
        PermissionLookup GetPermissionsLookupForRole(int tenantId, int roleId);
        Task BuildLoggedInTenantPermissionsCache();
        Task UpdateCacheForNewRoleForLoggedInTenant(int roleId, int roleType);
        void UpdateCacheForDeletedRoleForLoggedInTenant(int roleId);

        Task UpdateCacheForRoleForLoggedInTenant(int roleId, int roleType, IList<string> componentActivities);
    }
}