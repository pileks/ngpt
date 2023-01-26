using Ngpt.Platform.Data.Attributes;

namespace Ngpt.Data.Enums
{
    public enum UseOfLanguageQuestionType
    {
        [Title("Multiple choice")]
        MultipleChoice = 1,
        [Title("Single gap")]
        SingleGap = 2,
        [Title("Drag and drop")]
        DragDrop = 3,
        [Title("Single answer")]
        SingleAnswer = 4
    }
}
