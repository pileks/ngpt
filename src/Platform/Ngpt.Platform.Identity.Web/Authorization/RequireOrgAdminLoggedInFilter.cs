using System.Linq;
using Augur.Security.Exceptions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;

namespace Ngpt.Platform.Identity.Web.Authorization
{
    public class RequireOrgAdminLoggedInFilter : IAuthorizationFilter
    {
        private readonly ILoggedInUserProvider loggedInUserProvider;
        private readonly System.String[] ignoredActions;

        public RequireOrgAdminLoggedInFilter(ILoggedInUserProvider loggedInUserProvider, string[] ignoredActions)
        {
            this.loggedInUserProvider = loggedInUserProvider;
            this.ignoredActions = ignoredActions;
        }

        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string actionName = controllerActionDescriptor?.ActionName;

            if (this.ignoredActions == null || !this.ignoredActions.Contains(actionName))
            {
                if (!this.loggedInUserProvider.User.IsOrgAdmin)
                {
                    throw new ForbiddenException();
                }
            }
        }
    }
}