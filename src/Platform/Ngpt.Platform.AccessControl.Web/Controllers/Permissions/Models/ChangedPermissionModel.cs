namespace Ngpt.Platform.AccessControl.Web.Controllers.Permissions.Models
{
    public class ChangedPermissionModel
    {
        public string Component { get; set; }
        public string Activity { get; set; }
        public bool IsAllowed { get; set; }
    }
}