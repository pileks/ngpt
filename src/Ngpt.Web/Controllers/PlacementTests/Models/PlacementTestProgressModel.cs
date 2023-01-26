using System.Collections.Generic;

namespace Ngpt.Web.Controllers.PlacementTests.Models
{
    public class PlacementTestProgressModel
    {
        public bool IsCompleted { get; set; }
        public double Rating { get; set; }
        public double Rd { get; set; }
        public double Vol { get; set; }

        public int PlacementTestId { get; set; }

        public ICollection<PlacementTestProgressQuestionModel> Questions { get; set; }
    }
}