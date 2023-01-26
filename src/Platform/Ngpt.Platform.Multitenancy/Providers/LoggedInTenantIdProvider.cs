using System;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;

namespace Ngpt.Platform.Multitenancy.Data.Providers
{
    public class LoggedInTenantIdProvider : ILoggedInTenantIdProvider
    {
        private readonly ILoggedInUserProvider loggedInUserProvider;

        public LoggedInTenantIdProvider(ILoggedInUserProvider loggedInUserProvider)
        {
            this.loggedInUserProvider = loggedInUserProvider;
        }

        public int TenantId => this.loggedInUserProvider.User.TenantId;
        public bool IsTenantLoggedIn =>
            this.loggedInUserProvider.IsUserLoggedIn;

        public void RequireTenantLoggedIn()
        {
            if (!this.IsTenantLoggedIn)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
