using Augur.Entity.Base.Entities;
using Augur.Entity.Interfaces.Base;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Data.Interfaces.Entities;
using System.Collections.Generic;

namespace Ngpt.Data.Entities.Questions.SingleGap
{
    public class SingleGapQuestion : AugurEntityWithId, ITenantEntity, IUserOwnedEntityWithUserId
    {
        public SingleGapQuestion()
        {
            this.Answers = new HashSet<SingleGapQuestionAnswer>();
        }

        public string TextBefore { get; set; }
        public string TextAfter { get; set; }

        public virtual ICollection<SingleGapQuestionAnswer> Answers { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual Tenant Tenant { get; set; }
        public int TenantId { get; set; }
    }
}
