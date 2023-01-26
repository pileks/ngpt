using System.Collections.Generic;
using Augur.Entity.Base.Entities;
using Augur.Entity.Interfaces.Base;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Data.Interfaces.Entities;

namespace Ngpt.Data.Entities.Questions.Listening
{
    public class ListeningQuestion : AugurEntityWithId, ITenantEntity, IUserOwnedEntityWithUserId
    {
        public string QuestionText { get; set; }

        public ListeningQuestionAnswerType AnswerType { get; set; }

        public ListeningQuestionAudio Audio { get; set; }
        public int AudioId { get; set; }

        public ICollection<ListeningQuestionAnswer> Answers { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual Tenant Tenant { get; set; }
        public int TenantId { get; set; }
    }
}
