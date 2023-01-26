using Microsoft.AspNetCore.Mvc;
using System;

namespace Augur.Web.Authorization
{
    public class AugurRequireUserLoggedInAttribute : TypeFilterAttribute
    {
        public AugurRequireUserLoggedInAttribute() : base(typeof(AugurRequireUserLoggedInFilter))
        {
            Arguments = new[] {new string[] { }};
        }

        public AugurRequireUserLoggedInAttribute(params string[] ignoredActions) : base(
            typeof(AugurRequireUserLoggedInFilter))
        {
            Arguments = new[] {ignoredActions};
        }


        public AugurRequireUserLoggedInAttribute(Type filterType) : base(filterType)
        {
            Arguments = new[] {new string[] { }};
        }

        public AugurRequireUserLoggedInAttribute(Type filterType, params string[] ignoredActions) : base(filterType)
        {
            Arguments = new[] {ignoredActions};
        }
    }
}