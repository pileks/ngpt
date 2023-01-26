using Augur.Entity.Base.Entities;
using Ngpt.Platform.Data.Entities.UploadedResources;

namespace Ngpt.Data.Entities.Questions.Reading
{
    public class ReadingQuestionAnswer : AugurEntityWithId
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int Ordinal { get; set; }

        public UploadedResource Image { get; set; }
        public int? ImageId { get; set; }

        public ReadingQuestion Question { get; set; }
        public int QuestionId { get; set; }
    }
}
