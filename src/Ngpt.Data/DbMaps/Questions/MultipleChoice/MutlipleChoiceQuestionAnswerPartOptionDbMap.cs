using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.MultipleChoice;

namespace Ngpt.Data.DbMaps.Questions.MultipleChoice
{
    public class MutlipleChoiceQuestionAnswerPartOptionDbMap : IEntityTypeConfiguration<MutlipleChoiceQuestionAnswerPartOption>
    {
        public void Configure(EntityTypeBuilder<MutlipleChoiceQuestionAnswerPartOption> builder)
        {
            builder.ToTable("MutlipleChoiceQuestionAnswerPartOptions");

            builder.HasOne(x => x.AnswerPart)
                .WithMany(y => y.Options)
                .HasForeignKey(x => x.AnswerPartId);
        }
    }
}