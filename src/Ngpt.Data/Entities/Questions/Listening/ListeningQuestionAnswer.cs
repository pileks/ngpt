using Augur.Entity.Base.Entities;
using Ngpt.Platform.Data.Entities.UploadedResources;

namespace Ngpt.Data.Entities.Questions.Listening
{
    public class ListeningQuestionAnswer : AugurEntityWithId
    {
        public bool IsCorrect { get; set; }
        public int Ordinal { get; set; }

        public string Text { get; set; }

        public UploadedResource Image { get; set; }
        public int? ImageId { get; set; }

        public ListeningQuestion Question { get; set; }
        public int QuestionId { get; set; }
    }
}
