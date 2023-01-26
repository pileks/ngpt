using System.Linq;
using Ngpt.Platform.Identity.Data.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ngpt.Platform.Identity.Web.Authorization
{
    public class RequireUserWithVerifiedEmailLoggedInFilter : IAuthorizationFilter
    {
        private readonly ILoggedInUserAccessManager loggedInUserAccessManager;
        private readonly System.String[] ignoredActions;

        public RequireUserWithVerifiedEmailLoggedInFilter(ILoggedInUserAccessManager loggedInUserAccessManager, string[] ignoredActions)
        {
            this.loggedInUserAccessManager = loggedInUserAccessManager;
            this.ignoredActions = ignoredActions;
        }

        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string actionName = controllerActionDescriptor?.ActionName;

            if (this.ignoredActions == null || !this.ignoredActions.Contains(actionName))
            {
                this.loggedInUserAccessManager.RequireUserWithVerifiedEmailLoggedIn();
            }
        }
    }
}