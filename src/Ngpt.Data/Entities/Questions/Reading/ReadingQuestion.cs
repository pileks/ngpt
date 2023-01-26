using System.Collections.Generic;
using Augur.Entity.Base.Entities;
using Augur.Entity.Interfaces.Base;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Data.Interfaces.Entities;

namespace Ngpt.Data.Entities.Questions.Reading
{
    public class ReadingQuestion : AugurEntityWithId, ITenantEntity, IUserOwnedEntityWithUserId
    {
        public ReadingQuestionText Text { get; set; }
        public int TextId { get; set; }

        public string QuestionText { get; set; }

        public ReadingQuestionAnswerType AnswerType { get; set; }

        public ICollection<ReadingQuestionAnswer> Answers { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual Tenant Tenant { get; set; }
        public int TenantId { get; set; }
    }
}
