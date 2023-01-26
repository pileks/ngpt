using Augur.Entity.Base.Entities;

namespace Ngpt.Data.Entities.Questions.MultipleChoice
{
    public class MutlipleChoiceQuestionAnswerPartOption : AugurEntityWithId
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public MultipleChoiceQuestionAnswerPart AnswerPart { get; set; }
        public int AnswerPartId { get; set; }
    }
}