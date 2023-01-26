using Ngpt.Data.Entities.Questions.Reading;
using System.Collections.Generic;

namespace Ngpt.Web.Controllers.PlacementTests.Models
{
    public class GetReadingTestResultModel
    {
        public ReadingQuestionText Text { get; set; }
        public List<ReadingQuestion> Questions { get; set; }
        public bool ShouldSkip { get; set; }
    }
}