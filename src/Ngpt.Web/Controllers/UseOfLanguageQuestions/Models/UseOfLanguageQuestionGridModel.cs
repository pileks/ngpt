using Ngpt.Data.Enums;

namespace Ngpt.Web.Controllers.UseOfLanguageQuestions.Models
{
    public class UseOfLanguageQuestionGridModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Level { get; set; }
        public string Author { get; set; }
        public string Organization { get; set; }
        public UseOfLanguageQuestionType Type { get; set; }
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }
    }
}