using Augur.Entity.Base.Entities;
using Ngpt.Platform.Data.Interfaces.Entities;

namespace Ngpt.Platform.Data.Entities.AccountInfos
{
    public partial class AccountInfo : AugurEntityWithId, IUserOwnedEntityWithUserId
    {
        public string Name { get; set; }
        public virtual Users.User User { get; set; }
        public int UserId { get; set; }

        public bool HasAcceptedTermsAndPrivacyPolicy { get; set; }
    }
}