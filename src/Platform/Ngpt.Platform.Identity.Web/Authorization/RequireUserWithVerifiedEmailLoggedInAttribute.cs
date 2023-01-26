using Augur.Web.Authorization;

namespace Ngpt.Platform.Identity.Web.Authorization
{
    public class RequireUserWithVerifiedEmailLoggedInAttribute : AugurRequireUserLoggedInAttribute
    {
        public RequireUserWithVerifiedEmailLoggedInAttribute() : base(typeof(RequireUserWithVerifiedEmailLoggedInFilter))
        {
        }

        public RequireUserWithVerifiedEmailLoggedInAttribute(params string[] ignoredActions) : base(typeof(RequireUserWithVerifiedEmailLoggedInFilter), ignoredActions)
        {
        }
    }
}