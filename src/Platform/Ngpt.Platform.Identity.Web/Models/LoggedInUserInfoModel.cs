using System.Collections.Generic;

namespace Ngpt.Platform.Identity.Web.Models
{
    public class LoggedInUserInfoModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool HasVerifiedEmail { get; set; }
        public bool IsSuperAdmin { get; set; }
        public IList<PermissionModel> Permissions { get; set; }
        public int RoleId { get; set; }
        public bool IsOrgAdmin { get; set; }
    }
}