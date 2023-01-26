using Ngpt.Data.Entities.Questions.Reading;

namespace Ngpt.Web.Controllers.ReadingQuestions.Models
{
    public class ReadingQuestionGridModel
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string Author { get; set; }
        public string Organization { get; set; }
    }
}