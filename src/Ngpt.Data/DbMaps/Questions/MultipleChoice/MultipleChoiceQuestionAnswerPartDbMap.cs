using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.MultipleChoice;

namespace Ngpt.Data.DbMaps.Questions.MultipleChoice
{
    public class MultipleChoiceQuestionAnswerPartDbMap : IEntityTypeConfiguration<MultipleChoiceQuestionAnswerPart>
    {
        public void Configure(EntityTypeBuilder<MultipleChoiceQuestionAnswerPart> builder)
        {
            builder.ToTable("MultipleChoiceQuestionAnswerParts");

            builder.HasMany(x => x.Options)
                .WithOne(y => y.AnswerPart)
                .HasForeignKey(y => y.AnswerPartId);
        }
    }
}