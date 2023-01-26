using Augur.Entity.Base.Entities;
using Ngpt.Platform.Data.Entities.AccessControl;

namespace Ngpt.Platform.Data.Entities.Roles
{
    public partial class Role : AugurEntityWithId
    {
        public string Name { get; set; }
        public RoleType Type { get; set; }
        public int Priority { get; set; }
    }
}
