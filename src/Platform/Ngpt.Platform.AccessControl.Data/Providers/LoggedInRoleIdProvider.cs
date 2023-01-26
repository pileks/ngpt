using System;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.AccessControl.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;

namespace Ngpt.Platform.AccessControl.Data.Providers
{
    public class LoggedInRoleIdProvider : ILoggedInRoleIdProvider
    {
        private readonly ILoggedInUserProvider loggedInUserProvider;

        public int RoleId => this.loggedInUserProvider.User.RoleId;
        public bool IsRoleLoggedIn =>
            this.loggedInUserProvider.IsUserLoggedIn;

        public LoggedInRoleIdProvider(ILoggedInUserProvider loggedInUserProvider)
        {
            this.loggedInUserProvider = loggedInUserProvider;
        }

        public void RequireRoleLoggedIn()
        {
            if (!this.IsRoleLoggedIn)
            {
                throw new InvalidOperationException("This action requires a logged in role.");
            }
        }
    }
}
