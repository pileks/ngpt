namespace Ngpt.Web.Controllers.PlacementTests.Models
{
    public class CompleteListeningTestRequestModel
    {
        public int PlacementTestId { get; set; }
        public int TotalAnswers { get; set; }
        public int CorrectAnswers { get; set; }
        public int AudioId { get; set; }
    }
}