namespace Augur.Security.Interfaces.Permissions.Providers
{
    public interface IAugurLoggedInRoleIdProvider
    {
        int RoleId { get; }
        bool IsRoleLoggedIn { get; }
        void RequireRoleLoggedIn();
    };
}