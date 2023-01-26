using System;

namespace Augur.Security.Permissions.Exceptions
{
    public class PermissionMissingException : Exception
    {
        public PermissionMissingException(string component, string activity)
        {
            this.Component = component;
            this.Activity = activity;
        }

        public string Component { get; set; }
        public string Activity { get; set; }
    }
}