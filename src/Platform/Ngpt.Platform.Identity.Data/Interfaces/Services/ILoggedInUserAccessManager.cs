namespace Ngpt.Platform.Identity.Data.Interfaces.Services
{
    public interface ILoggedInUserAccessManager : Augur.Security.Interfaces.Services.ILoggedInUserAccessManager
    {
        void RequireUserWithVerifiedEmailLoggedIn();
    }
}