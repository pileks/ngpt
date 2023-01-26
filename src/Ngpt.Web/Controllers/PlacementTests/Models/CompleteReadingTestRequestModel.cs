namespace Ngpt.Web.Controllers.PlacementTests.Models
{
    public class CompleteReadingTestRequestModel
    {
        public int PlacementTestId { get; set; }
        public int TextId { get; set; }
        public int TotalAnswers { get; set; }
        public int CorrectAnswers { get; set; }
    }
}