using Augur.Entity.Base.Entities;
using Augur.Entity.Interfaces.Base;
using Ngpt.Data.Entities.Questions.DragDrop;
using Ngpt.Data.Entities.Questions.MultipleChoice;
using Ngpt.Data.Entities.Questions.SingleAnswer;
using Ngpt.Data.Entities.Questions.SingleGap;
using Ngpt.Data.Enums;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Data.Interfaces.Entities;

namespace Ngpt.Data.Entities.Questions
{
    public class UseOfLanguageQuestion : AugurEntityWithId, ITenantEntity, IUserOwnedEntityWithUserId
    {
        public Level Level { get; set; }
        public int LevelId { get; set; }

        public Language Language { get; set; }
        public int LanguageId { get; set; }

        public string Title { get; set; }
        public bool Approved { get; set; }
        public UseOfLanguageQuestionType Type { get; set; }

        public int? MultipleChoiceQuestionId { get; set; }
        public virtual MultipleChoiceQuestion MultipleChoiceQuestion { get; set; }

        public int? SingleGapQuestionId { get; set; }
        public virtual SingleGapQuestion SingleGapQuestion { get; set; }

        public int? DragDropQuestionId { get; set; }
        public virtual DragDropQuestion DragDropQuestion { get; set; }

        public int? SingleAnswerQuestionId { get; set; }
        public virtual SingleAnswerQuestion SingleAnswerQuestion { get; set; }

        public virtual Instruction Instruction { get; set; }
        public int InstructionId { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual Tenant Tenant { get; set; }
        public int TenantId { get; set; }

        public User Approver { get; set; }
        public int? ApproverId { get; set; }
    }
}
