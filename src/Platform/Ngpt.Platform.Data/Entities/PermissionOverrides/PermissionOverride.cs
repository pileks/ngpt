using Augur.BackendToFrontendExporter;
using Augur.Entity.Base.Entities;
using Role = Ngpt.Platform.Data.Entities.Roles.Role;

namespace Ngpt.Platform.Data.Entities.PermissionOverrides
{
    [DisableExportToFrontend]
    public partial class PermissionOverride : AugurEntityWithId
    {
        public virtual string Component { get; set; }
        public virtual string Activity { get; set; }
        public virtual bool IsAllowed { get; set; }

        public virtual Roles.Role Role { get; set; }
        public virtual int RoleId { get; set; }
    }
}
