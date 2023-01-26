using Augur.BackendToFrontendExporter;
using Augur.Entity.Base.Entities;
using Augur.Entity.Interfaces.Entities;

namespace Augur.Entity.Entities
{
    public class AugurUser : AugurEntityWithId, IAugurUser
    {
        public AugurUser()
        {
        }

        public virtual string Email { get; set; }

        [DisableExportToFrontend] public virtual string Password { get; set; }

        public virtual bool IsActive { get; set; }
    }
}