using Augur.Web.Authorization;

namespace Ngpt.Platform.Identity.Web.Authorization
{
    public class RequireUserLoggedInAttribute : AugurRequireUserLoggedInAttribute
    {
        public RequireUserLoggedInAttribute() : base(typeof(RequireUserLoggedInFilter))
        {
        }

        public RequireUserLoggedInAttribute(params string[] ignoredActions) : base(typeof(RequireUserLoggedInFilter), ignoredActions)
        {
        }
    }
}