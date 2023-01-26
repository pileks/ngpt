using Ngpt.Platform.Identity.Data.Interfaces.Providers;

namespace Ngpt.Platform.Identity.Data.Providers
{
    public class LoggedInUserInfoProvider : ILoggedInUserInfoProvider
    {
        private readonly ILoggedInUserProvider loggedInUserProvider;

        public LoggedInUserInfoProvider(ILoggedInUserProvider loggedInUserProvider)
        {
            this.loggedInUserProvider = loggedInUserProvider;
        }

        public bool IsSuperAdmin => this.loggedInUserProvider.IsUserLoggedIn &&
            this.loggedInUserProvider.User.IsSuperAdmin;
        public bool IsLoggedIn => this.loggedInUserProvider.IsUserLoggedIn;

        public bool HasVerifiedEmail => this.loggedInUserProvider.User.HasVerifiedEmail;
    }
}
