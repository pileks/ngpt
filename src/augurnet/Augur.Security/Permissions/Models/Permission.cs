namespace Augur.Security.Permissions.Models
{
    public class Permission
    {
        public virtual string Component { get; set; }
        public virtual string Activity { get; set; }
        public virtual bool IsAllowed { get; set; }

        public override string ToString()
        {
            return $"{this.Component}.{this.Activity}={this.IsAllowed}";
        }
    }
}