using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.MultipleChoice;

namespace Ngpt.Data.DbMaps.Questions.MultipleChoice
{
    public class MultipleChoiceQuestionPartDbMap : IEntityTypeConfiguration<MultipleChoiceQuestionPart>
    {
        public void Configure(EntityTypeBuilder<MultipleChoiceQuestionPart> builder)
        {
            builder.ToTable("MultipleChoiceQuestionParts");

            builder.HasOne(x => x.AnswerPart)
                .WithMany()
                .HasForeignKey(x => x.AnswerPartId);

            builder.HasOne(x => x.TextPart)
                .WithMany()
                .HasForeignKey(x => x.TextPartId);
        }
    }
}