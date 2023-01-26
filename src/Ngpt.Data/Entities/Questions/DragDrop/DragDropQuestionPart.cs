using Augur.Entity.Base.Entities;

namespace Ngpt.Data.Entities.Questions.DragDrop
{
    public class DragDropQuestionPart: AugurEntityWithId
    {
        public int Ordinal { get; set; }
        public string Text { get; set; }
        public bool IsDraggable { get; set; }

        public DragDropQuestion Question { get; set; }
        public int QuestionId { get; set; }
    }
}