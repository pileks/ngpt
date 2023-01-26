using Augur.Entity.Interfaces.Base;
using Ngpt.Platform.Data.Entities.Tenants;

namespace Ngpt.Platform.Data.Entities.Users
{
    public partial class User : ITenantEntity
    {
        public bool IsOrgAdmin { get; set; }
    }
}