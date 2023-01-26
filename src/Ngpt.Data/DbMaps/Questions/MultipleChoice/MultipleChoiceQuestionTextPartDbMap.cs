using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.MultipleChoice;

namespace Ngpt.Data.DbMaps.Questions.MultipleChoice
{
    public class MultipleChoiceQuestionTextPartDbMap : IEntityTypeConfiguration<MultipleChoiceQuestionTextPart>
    {
        public void Configure(EntityTypeBuilder<MultipleChoiceQuestionTextPart> builder)
        {
            builder.ToTable("MultipleChoiceQuestionTextParts");
        }
    }
}