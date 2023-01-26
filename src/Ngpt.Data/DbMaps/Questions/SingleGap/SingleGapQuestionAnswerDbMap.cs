using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.SingleGap;

namespace Ngpt.Data.DbMaps.Questions.SingleGap
{
    public class SingleGapQuestionAnswerDbMap : IEntityTypeConfiguration<SingleGapQuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<SingleGapQuestionAnswer> builder)
        {
            builder.ToTable("SingleGapQuestionAnswers");

            builder.HasOne(x => x.SingleGapQuestion)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.SingleGapQuestionId);
        }
    }
}
