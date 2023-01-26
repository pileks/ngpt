using System.Collections.Generic;

namespace Ngpt.Platform.AccessControl.Web.Controllers.Permissions.Models
{
    public class PermissionComponentModel
    {
        public string Name { get; set; }
        public bool IsAllowed { get; set; }

        public ICollection<PermissionActivityModel> Activities { get; set; }
    }
}