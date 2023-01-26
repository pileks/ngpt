using Augur.BackendToFrontendExporter;
using Augur.Entity.Base.Entities;
using Ngpt.Platform.Data.Interfaces.Entities;

namespace Ngpt.Platform.Data.Entities.ChangePasswordRequests
{
    [DisableExportToFrontend]
    public class ChangePasswordRequest : AugurEntityWithId, IUserOwnedEntityWithUserId
    {
        public string Uid { get; set; }

        public int SentEmailId { get; set; }
        public virtual SentEmail SentEmail { get; set; }

        public int UserId { get; set; }
        public virtual Users.User User { get; set; }
    }
}
