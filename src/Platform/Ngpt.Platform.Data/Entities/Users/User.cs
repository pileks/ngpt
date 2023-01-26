using Augur.Entity.Entities;
using Ngpt.Platform.Data.Interfaces.Entities;
using AccountInfo = Ngpt.Platform.Data.Entities.AccountInfos.AccountInfo;

namespace Ngpt.Platform.Data.Entities.Users
{
    public partial class User : AugurUser, IUserOwnedEntityWithUserId
    {
        public bool IsSuperAdmin { get; set; }
        public bool HasVerifiedEmail { get; set; }
        public virtual AccountInfos.AccountInfo AccountInfo { get; set; }

        public int UserId
        {
            get => this.Id;
            set => this.Id = value;
        }
    }
}