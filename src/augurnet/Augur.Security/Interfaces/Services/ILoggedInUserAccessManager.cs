namespace Augur.Security.Interfaces.Services
{
    public interface ILoggedInUserAccessManager
    {
        void RequireUserLoggedIn();
        void RequireSuperAdminLoggedIn();
    }
}