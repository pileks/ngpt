using Ngpt.Data.Entities.Questions;
using System.Collections.Generic;

namespace Ngpt.Web.Controllers.PlacementTests.Models
{
    public class PlacementTestQuestionsModel
    {
        public ICollection<UseOfLanguageQuestion> Questions { get; set; }
    }
}