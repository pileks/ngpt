using System.Collections.Generic;
using Augur.Security.Permissions.Models;

namespace Augur.Security.Interfaces.Permissions.Services
{
    public interface IPermissionsProvider
    {
        IEnumerable<Permission> GetPermissionsForRole(int roleId);
        IDictionary<string, Permission> GetPermissionsLookupForRole(int roleId);
    }
}