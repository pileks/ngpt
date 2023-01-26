using Augur.Entity.Base.Entities;
using Ngpt.Data.Entities.Questions.Listening;
using Ngpt.Data.Entities.Questions.Reading;
using System;

namespace Ngpt.Data.Entities
{
    public class PlacementTest : AugurEntityWithId
    {
        public double Rating { get; set; }
        public double Rd { get; set; }
        public double Vol { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public DateTime StartedOn { get; set; }
        public DateTime? CompletedOn { get; set; }

        public Level ReportedLevel { get; set; }
        public int? ReportedLevelId { get; set; }

        public bool ShouldTestReading { get; set; }
        public bool ShouldTestListening { get; set; }

        public int? ReadingTextCorrectAnswers { get; set; }
        public int? ReadingTextTotalAnswers { get; set; }
        public int? ListeningAudioCorrectAnswers { get; set; }
        public int? ListeningAudioTotalAnswers { get; set; }


        public int? ReadingQuestionTextId { get; set; }
        public ReadingQuestionText ReadingQuestionText { get; set; }

        public int? ListeningQuestionAudioId { get; set; }
        public ListeningQuestionAudio ListeningQuestionAudio { get; set; }

    }
}
