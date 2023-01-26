using Augur.Web.Authorization;
using Ngpt.Platform.Identity.Data.Interfaces.Services;

namespace Ngpt.Platform.Identity.Web.Authorization
{
    public class RequireSuperAdminLoggedInFilter : AugurRequireSuperAdminLoggedInFilter
    {
        public RequireSuperAdminLoggedInFilter(ILoggedInUserAccessManager loggedInUserAccessManager, string[] ignoredActions) : base(loggedInUserAccessManager, ignoredActions)
        {
        }
    }
}