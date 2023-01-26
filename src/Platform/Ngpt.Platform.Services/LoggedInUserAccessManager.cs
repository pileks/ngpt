using Augur.Security.Exceptions;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Data.Interfaces.Services;

namespace Ngpt.Platform.Services
{
    public class LoggedInUserAccessManager : ILoggedInUserAccessManager
    {
        private readonly ILoggedInUserInfoProvider loggedInUserInfoProvider;

        public LoggedInUserAccessManager(ILoggedInUserInfoProvider loggedInUserInfoProvider)
        {
            this.loggedInUserInfoProvider = loggedInUserInfoProvider;
        }

        public void RequireUserLoggedIn()
        {
            if (!this.loggedInUserInfoProvider.IsLoggedIn)
            {
                throw new UnauthorizedException();
            }
        }

        public void RequireSuperAdminLoggedIn()
        {
            this.RequireUserLoggedIn();

            if (!this.loggedInUserInfoProvider.IsSuperAdmin)
            {
                throw new ForbiddenException();
            } 
        }

        public void RequireUserWithVerifiedEmailLoggedIn()
        {
            this.RequireUserLoggedIn();

            if (!this.loggedInUserInfoProvider.HasVerifiedEmail)
            {
                throw new ForbiddenException();
            }
        }
    }
}
