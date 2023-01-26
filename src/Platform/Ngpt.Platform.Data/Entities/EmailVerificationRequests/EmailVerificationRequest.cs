using System;
using Augur.BackendToFrontendExporter;
using Augur.Entity.Base.Entities;
using Ngpt.Platform.Data.Interfaces.Entities;

namespace Ngpt.Platform.Data.Entities.EmailVerificationRequests
{
    [DisableExportToFrontend]
    public class EmailVerificationRequest : AugurEntityWithId, IUserOwnedEntityWithUserId
    {
        public string Code { get; set; }
        public int NumberOfVerificationAttempts { get; set; }
        public DateTime CreatedOn { get; set; }
        public int SentEmailId { get; set; }
        public virtual SentEmail SentEmail { get; set; }

        public int UserId { get; set; }
        public virtual Users.User User { get; set; }
    }
}
