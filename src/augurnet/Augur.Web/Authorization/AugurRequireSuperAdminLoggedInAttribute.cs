using Microsoft.AspNetCore.Mvc;
using System;

namespace Augur.Web.Authorization
{
    public class AugurRequireSuperAdminLoggedInAttribute : TypeFilterAttribute
    {
        public AugurRequireSuperAdminLoggedInAttribute() : base(typeof(AugurRequireSuperAdminLoggedInFilter))
        {
            Arguments = new[] {new string[] { }};
        }

        public AugurRequireSuperAdminLoggedInAttribute(params string[] ignoredActions) : base(
            typeof(AugurRequireSuperAdminLoggedInFilter))
        {
            Arguments = new[] {ignoredActions};
        }

        public AugurRequireSuperAdminLoggedInAttribute(Type filterType) : base(filterType)
        {
            Arguments = new[] {new string[] { }};
        }

        public AugurRequireSuperAdminLoggedInAttribute(Type filterType, params string[] ignoredActions) : base(
            filterType)
        {
            Arguments = new[] {ignoredActions};
        }
    }
}