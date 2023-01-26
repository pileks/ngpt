using System.Collections.Generic;
using Augur.Entity.Base.Entities;

namespace Ngpt.Data.Entities.Questions.MultipleChoice
{
    public class MultipleChoiceQuestionAnswerPart : AugurEntityWithId
    {
        public ICollection<MutlipleChoiceQuestionAnswerPartOption> Options { get; set; }
    }
}