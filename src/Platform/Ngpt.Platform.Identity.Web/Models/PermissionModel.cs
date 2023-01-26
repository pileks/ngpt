namespace Ngpt.Platform.Identity.Web.Models
{
    public class PermissionModel
    {
        public string Component { get; set; }
        public string Activity { get; set; }
        public bool IsAllowed { get; set; }

        public override string ToString()
        {
            return $"{this.Component}.{this.Activity}={this.IsAllowed}";
        }
    }
}
