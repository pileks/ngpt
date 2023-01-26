using Ngpt.Data.Enums;

namespace Ngpt.Web.Controllers.Instructions.Models
{
    public class InstructionGridModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public UseOfLanguageQuestionType QuestionType { get; set; }
        public string Language { get; set; }
    }
}