using Ngpt.Data.Entities.Questions.Listening;
using System.Collections.Generic;

namespace Ngpt.Web.Controllers.PlacementTests.Models
{
    public class GetListeningTestResultModel
    {
        public ListeningQuestionAudio Audio { get; set; }
        public List<ListeningQuestion> Questions { get; set; }
        public bool ShouldSkip { get; set; }
    }
}