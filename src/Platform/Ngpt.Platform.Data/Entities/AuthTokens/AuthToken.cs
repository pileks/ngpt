using System;
using Augur.BackendToFrontendExporter;
using Augur.Entity.Base.Entities;
using Ngpt.Platform.Data.Interfaces.Entities;

namespace Ngpt.Platform.Data.Entities.AuthTokens
{
    [DisableExportToFrontend]
    public partial class AuthToken : AugurEntityWithId, IUserOwnedEntityWithUserId
    {
        public AuthToken()
        {
        }

        public DateTime ExpirationDate { get; set; }
        public string Guid { get; set; }
        public int UserId { get; set; }
        public virtual Platform.Data.Entities.Users.User User { get; set; }
    }
}