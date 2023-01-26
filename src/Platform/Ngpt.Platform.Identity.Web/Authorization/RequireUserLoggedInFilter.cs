using Augur.Web.Authorization;
using Ngpt.Platform.Identity.Data.Interfaces.Services;

namespace Ngpt.Platform.Identity.Web.Authorization
{
    public class RequireUserLoggedInFilter : AugurRequireUserLoggedInFilter
    {
        public RequireUserLoggedInFilter(ILoggedInUserAccessManager loggedInUserAccessManager, string[] ignoredActions) : base(loggedInUserAccessManager, ignoredActions)
        {
        }
    }
}