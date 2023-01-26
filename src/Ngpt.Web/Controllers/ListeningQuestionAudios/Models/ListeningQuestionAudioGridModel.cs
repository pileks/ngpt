namespace Ngpt.Web.Controllers.ListeningQuestionAudios.Models
{
    public class ListeningQuestionAudioGridModel
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
        public bool FileAdded { get; set; }
    }
}