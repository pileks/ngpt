using Augur.Data.Interfaces.Providers;
using Augur.Security.Interfaces.Permissions.Providers;
using Augur.Security.Interfaces.Permissions.Services;
using Augur.Security.Permissions.Services;
using Ngpt.Platform.AccessControl.Interfaces.Services;

namespace Ngpt.Platform.AccessControl.Services
{
    public class PermissionsService : AugurPermissionsService, IPermissionsService
    {
        public PermissionsService(IAugurLoggedInUserInfoProvider loggedInUserInfoProvider, IAugurLoggedInRoleIdProvider loggedInRoleIdProvider,
            IPermissionsProvider permissionsProvider) : base(loggedInUserInfoProvider, loggedInRoleIdProvider, permissionsProvider)
        {
        }
    }
}
