using Augur.Entity.Base.Entities;
using Ngpt.Platform.Data.Entities.UploadedResources;

namespace Ngpt.Data.Entities.Questions.SingleAnswer
{
    public class SingleAnswerQuestionAnswer : AugurEntityWithId
    {
        public bool IsCorrect { get; set; }

        public string Text { get; set; }

        public UploadedResource Image { get; set; }
        public int? ImageId { get; set; }

        public SingleAnswerQuestion Question { get; set; }
        public int QuestionId { get; set; }
    }
}