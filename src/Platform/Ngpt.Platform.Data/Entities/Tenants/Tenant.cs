using Augur.Entity.Base.Entities;
using Augur.Entity.Interfaces.Base;
using Ngpt.Platform.Data.Entities.Users;

namespace Ngpt.Platform.Data.Entities.Tenants
{
    public partial class Tenant : AugurEntityWithId, ITenantEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
      
        public int TenantId
        {
            get => this.Id;
            set => this.Id = value;
        }
    }
}
