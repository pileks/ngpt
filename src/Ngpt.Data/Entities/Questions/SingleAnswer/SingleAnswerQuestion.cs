using Augur.Entity.Base.Entities;
using Augur.Entity.Interfaces.Base;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Data.Interfaces.Entities;
using System.Collections.Generic;

namespace Ngpt.Data.Entities.Questions.SingleAnswer
{
    public class SingleAnswerQuestion : AugurEntityWithId, ITenantEntity, IUserOwnedEntityWithUserId
    {
        public string QuestionText { get; set; }

        public SingleAnswerQuestionAnswerType AnswerType { get; set; }

        public ICollection<SingleAnswerQuestionAnswer> Answers { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual Tenant Tenant { get; set; }
        public int TenantId { get; set; }
    }
}
