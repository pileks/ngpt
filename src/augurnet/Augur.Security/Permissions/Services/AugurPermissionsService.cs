using System;
using System.Linq.Expressions;
using System.Security;
using Augur.Data.Interfaces.Providers;
using Augur.Security.Interfaces.Permissions.Providers;
using Augur.Security.Interfaces.Permissions.Services;
using Augur.Security.Permissions.Components;
using Augur.Security.Permissions.Exceptions;

namespace Augur.Security.Permissions.Services
{
    public class AugurPermissionsService : IAugurPermissionsService
    {
        private readonly IAugurLoggedInUserInfoProvider loggedInUserInfoProvider;
        private readonly IAugurLoggedInRoleIdProvider loggedInRoleIdProvider;
        private readonly IPermissionsProvider permissionsProvider;

        public AugurPermissionsService(IAugurLoggedInUserInfoProvider loggedInUserInfoProvider,
            IAugurLoggedInRoleIdProvider loggedInRoleIdProvider,
            IPermissionsProvider permissionsProvider)
        {
            this.loggedInUserInfoProvider = loggedInUserInfoProvider;
            this.loggedInRoleIdProvider = loggedInRoleIdProvider;
            this.permissionsProvider = permissionsProvider;
        }

        public virtual bool CanPerform(Expression<Func<Activity>> componentActivityExpression)
        {
            if (this.loggedInUserInfoProvider.IsSuperAdmin)
            {
                return true;
            }

            if (!this.loggedInRoleIdProvider.IsRoleLoggedIn)
            {
                throw new SecurityException();
            }

            var componentActivity = this.GetComponentActivityFromExpression(componentActivityExpression);

            if (!this.CanAccess(componentActivity.ComponentType))
            {
                return false;
            }

            var loggedInUserRolePermissionsLookup =
                this.permissionsProvider.GetPermissionsLookupForRole(this.loggedInRoleIdProvider.RoleId);
            var permissionModel =
                loggedInUserRolePermissionsLookup[componentActivity.Component + "." + componentActivity.Activity];

            if (permissionModel != null)
            {
                return permissionModel.IsAllowed;
            }
            else
            {
                throw new PermissionMissingException(componentActivity.Component, componentActivity.Activity);
            }
        }

        public virtual void ShouldPerform(Expression<Func<Activity>> componentActivityExpression)
        {
            if (!this.CanPerform(componentActivityExpression))
            {
                var componentActivity = this.GetComponentActivityFromExpression(componentActivityExpression);

                throw new CannotPerformActivityException(componentActivity.Component, componentActivity.Activity);
            }
        }

        public virtual bool CanAccess<T>() where T : Component
        {
            return this.CanAccess(typeof(T));
        }

        public virtual void ShouldAccess<T>() where T : Component
        {
            if (!this.CanAccess<T>())
            {
                var componentName = typeof(T).Name;

                throw new CannotPerformActivityException(componentName, null);
            }
        }

        protected virtual bool CanAccess(Type t)
        {
            if (this.loggedInUserInfoProvider.IsSuperAdmin)
            {
                return true;
            }

            if (!this.loggedInRoleIdProvider.IsRoleLoggedIn)
            {
                throw new SecurityException();
            }

            var component = t.Name;

            var loggedInUserRolePermissionsLookup =
                this.permissionsProvider.GetPermissionsLookupForRole(this.loggedInRoleIdProvider.RoleId);

            var permissionModel = loggedInUserRolePermissionsLookup[component];

            if (permissionModel != null)
            {
                return permissionModel.IsAllowed;
            }
            else
            {
                throw new PermissionMissingException(t.Name, null);
            }
        }

        private ComponentActivity GetComponentActivityFromExpression(
            Expression<Func<Activity>> componentActivityExpression)
        {
            var member = (componentActivityExpression.Body as MemberExpression).Member;

            var activity = member.Name;

            var componentType = member.ReflectedType.GenericTypeArguments.Length > 0
                ? member.ReflectedType.GenericTypeArguments[0]
                : member.ReflectedType.BaseType.GenericTypeArguments[0];

            var component = componentType.Name;

            return new ComponentActivity(component, activity, componentType);
        }

        private class ComponentActivity
        {
            public ComponentActivity(string component, string activity, Type componentType)
            {
                this.Component = component;
                this.Activity = activity;
                this.ComponentType = componentType;
            }

            public string Component { get; }
            public string Activity { get; }
            public Type ComponentType { get; }
        }
    }
}