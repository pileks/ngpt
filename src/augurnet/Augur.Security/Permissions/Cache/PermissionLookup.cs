using System.Collections.Concurrent;
using Augur.Security.Permissions.Models;

namespace Augur.Security.Permissions.Cache
{
    public class PermissionLookup : ConcurrentDictionary<string, Permission>
    {
    }
}