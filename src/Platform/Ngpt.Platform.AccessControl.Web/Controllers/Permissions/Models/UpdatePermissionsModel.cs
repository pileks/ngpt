using System.Collections.Generic;

namespace Ngpt.Platform.AccessControl.Web.Controllers.Permissions.Models
{
    public class UpdatePermissionsModel
    {
        public int RoleId { get; set; }
        public ICollection<ChangedPermissionModel> Permissions { get; set; }
    }
}