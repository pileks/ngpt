using Augur.Entity.Base.Entities;

namespace Ngpt.Data.Entities.Questions.MultipleChoice
{
    public class MultipleChoiceQuestionPart : AugurEntityWithId
    {
        public int Ordinal { get; set; }

        public MultipleChoiceQuestionTextPart TextPart { get; set; }
        public int? TextPartId { get; set; }
        
        public MultipleChoiceQuestionAnswerPart AnswerPart { get; set; }
        public int? AnswerPartId { get; set; }

        public MultipleChoiceQuestion Question { get; set; }
        public int QuestionId { get; set; }
    }
}