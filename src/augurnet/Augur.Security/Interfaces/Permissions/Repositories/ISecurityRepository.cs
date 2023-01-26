using System.Collections.Generic;
using System.Threading.Tasks;
using Augur.Security.Permissions.Models;

namespace Augur.Security.Interfaces.Permissions.Repositories
{
    public interface IAccessControlRepository
    {
        Task<IList<int>> GetAllTenantIdsAsync();
        Task<IList<Permission>> GetPermissionsForTenantAndRoleAsync(int tenantId, int roleId, int roleType);
        Task<IList<(int RoleId, ICollection<Permission>)>> GetRoleIdsWithPermissionsForTenantAsync(int tenantId);

        Task<IList<Permission>> GetPermissionsForTenantAndRoleAsync(int tenantId, int roleId, int roleType,
            IList<string> componentActivities);
    }
}