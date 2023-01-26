namespace Ngpt.Web.Controllers.ReadingQuestionTexts.Models
{
    public class ReadingQuestionTextGridModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Level { get; set; }
        public string Author { get; set; }
        public string Organization { get; set; }
        public int NumberOfQuestions { get; set; }
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }
    }
}