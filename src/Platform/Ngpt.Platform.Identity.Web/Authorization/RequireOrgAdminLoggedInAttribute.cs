using Augur.Web.Authorization;

namespace Ngpt.Platform.Identity.Web.Authorization
{
    public class RequireOrgAdminLoggedInAttribute : AugurRequireUserLoggedInAttribute
    {
        public RequireOrgAdminLoggedInAttribute() : base(typeof(RequireOrgAdminLoggedInFilter))
        {
        }

        public RequireOrgAdminLoggedInAttribute(params string[] ignoredActions) : base(typeof(RequireOrgAdminLoggedInFilter), ignoredActions)
        {
        }
    }
}