using Augur.Entity.Interfaces.Base;
using Ngpt.Platform.Data.Entities.Tenants;

namespace Ngpt.Platform.Data.Entities.Users
{
    public partial class User : ITenantEntity
    {
        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}