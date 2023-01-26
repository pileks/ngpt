using Augur.Web.Authorization;

namespace Ngpt.Platform.Identity.Web.Authorization
{
    public class RequireSuperAdminLoggedInAttribute : AugurRequireSuperAdminLoggedInAttribute
    {
        public RequireSuperAdminLoggedInAttribute() : base(typeof(RequireSuperAdminLoggedInFilter))
        {
        }

        public RequireSuperAdminLoggedInAttribute(params string[] ignoredActions) : base(typeof(RequireSuperAdminLoggedInFilter), ignoredActions)
        {
        }
    }
}