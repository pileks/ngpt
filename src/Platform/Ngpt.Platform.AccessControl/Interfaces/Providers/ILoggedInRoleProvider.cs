using Ngpt.Platform.Data.Entities.Roles;

namespace Ngpt.Platform.AccessControl.Interfaces.Providers
{
    public interface ILoggedInRoleProvider
    {
        int? RoleId { get; }
        Role Role { get; }

        bool IsRoleLoggedIn { get; }
    }
}
