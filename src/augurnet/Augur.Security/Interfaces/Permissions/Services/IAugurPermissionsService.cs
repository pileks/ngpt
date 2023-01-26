using System;
using System.Linq.Expressions;
using Augur.Security.Permissions.Components;

namespace Augur.Security.Interfaces.Permissions.Services
{
    public interface IAugurPermissionsService
    {
        bool CanPerform(Expression<Func<Activity>> componentActivityExpression);
        void ShouldPerform(Expression<Func<Activity>> componentActivityExpression);
        bool CanAccess<T>() where T : Component;
        void ShouldAccess<T>() where T : Component;
    }
}