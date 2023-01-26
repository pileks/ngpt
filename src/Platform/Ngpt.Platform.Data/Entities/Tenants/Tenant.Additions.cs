using Augur.Entity.Base.Entities;
using Augur.Entity.Interfaces.Base;
using Ngpt.Platform.Data.Entities.Users;

namespace Ngpt.Platform.Data.Entities.Tenants
{
    public partial class Tenant : AugurEntityWithId, ITenantEntity
    {
        public virtual User OrgOwner { get; set; }
        public int? OrgOwnerId { get; set; }
    }
}
