using System.Collections.Generic;
using System.Linq;
using Augur.Data.Interfaces.Providers;
using Augur.Security.Interfaces.Permissions.Services;
using Augur.Security.Permissions.Cache;
using Augur.Security.Permissions.Models;

namespace Augur.Security.Permissions.Services
{
    public class PermissionsProvider : IPermissionsProvider
    {
        private readonly TenantPermissionsCache permissionsCache;
        private readonly IAugurLoggedInTenantIdProvider tenantIdProvider;

        public PermissionsProvider(TenantPermissionsCache permissionsCache,
            IAugurLoggedInTenantIdProvider tenantIdProvider)
        {
            this.permissionsCache = permissionsCache;
            this.tenantIdProvider = tenantIdProvider;
        }

        public IEnumerable<Permission> GetPermissionsForRole(int roleId)
        {
            return this.GetPermissionsLookupForRole(roleId).Select(a => a.Value);
        }

        public IDictionary<string, Permission> GetPermissionsLookupForRole(int roleId)
        {
            return this.permissionsCache.GetPermissionsLookupForRole(this.tenantIdProvider.TenantId, roleId);
        }
    }
}