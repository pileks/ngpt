using Augur.Entity.Base.Entities;

namespace Ngpt.Data.Entities.Questions.SingleGap
{
    public class SingleGapQuestionAnswer : AugurEntityWithId
    {
        public string Text { get; set; }
        public bool IsCaseSensitive { get; set; }

        public virtual SingleGapQuestion SingleGapQuestion { get; set; }
        public int SingleGapQuestionId { get; set; }
    }
}
