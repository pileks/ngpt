using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Linq;
using Augur.Security.Interfaces.Services;

namespace Augur.Web.Authorization
{
    public class AugurRequireUserLoggedInFilter : IAuthorizationFilter
    {
        private readonly ILoggedInUserAccessManager securityService;
        private readonly string[] ignoredActions;

        public AugurRequireUserLoggedInFilter(ILoggedInUserAccessManager securityService, string[] ignoredActions)
        {
            this.securityService = securityService;
            this.ignoredActions = ignoredActions;
        }

        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string controllerName = controllerActionDescriptor?.ControllerName;
            string actionName = controllerActionDescriptor?.ActionName;

            if (ignoredActions == null || !ignoredActions.Contains(actionName))
            {
                this.securityService.RequireUserLoggedIn();
            }
        }
    }
}