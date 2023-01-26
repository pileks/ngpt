using System.Collections.Concurrent;

namespace Augur.Security.Permissions.Cache
{
    public class RolePermissionLookup : ConcurrentDictionary<int, PermissionLookup>
    {
    }
}