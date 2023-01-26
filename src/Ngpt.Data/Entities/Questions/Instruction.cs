using Augur.Entity.Base.Entities;
using Ngpt.Data.Enums;

namespace Ngpt.Data.Entities.Questions
{
    public class Instruction : AugurEntityWithId
    {
        public string Text { get; set; }

        public UseOfLanguageQuestionType QuestionType { get; set; }

        public Language Language { get; set; }
        public int LanguageId { get; set; }
    }
}
