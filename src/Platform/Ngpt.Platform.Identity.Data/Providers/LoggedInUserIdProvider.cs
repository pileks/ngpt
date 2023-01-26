using System;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;

namespace Ngpt.Platform.Identity.Data.Providers
{
    public class LoggedInUserIdProvider : ILoggedInUserIdProvider
    {
        private readonly ILoggedInUserProvider loggedInUserProvider;

        public int UserId => this.loggedInUserProvider.User.Id;
        public bool IsUserLoggedIn =>
            this.loggedInUserProvider.IsUserLoggedIn;

        public LoggedInUserIdProvider(ILoggedInUserProvider loggedInUserProvider)
        {
            this.loggedInUserProvider = loggedInUserProvider;
        }

        public void RequireUserLoggedIn()
        {
            if (!this.IsUserLoggedIn)
            {
                throw new InvalidOperationException("This action requires a logged in user.");
            }
        }
    }
}
