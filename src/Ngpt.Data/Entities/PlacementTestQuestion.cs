using Augur.Entity.Base.Entities;
using Ngpt.Data.Entities.Questions;

namespace Ngpt.Data.Entities
{
    public class PlacementTestQuestion : AugurEntityWithId
    {
        public double Rating { get; set; }
        public double Rd { get; set; }
        public double Vol { get; set; }

        public bool IsAnsweredCorrectly { get; set; }

        public int QuestionId { get; set; }
        public UseOfLanguageQuestion Question { get; set; }

        public int PlacementTestId { get; set; }
        public PlacementTest PlacementTest { get; set; }

    }
}
