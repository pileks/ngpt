using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.SingleGap;

namespace Ngpt.Data.DbMaps.Questions.SingleGap
{
    public class SingleGapQuestionDbMap : IEntityTypeConfiguration<SingleGapQuestion>
    {
        public void Configure(EntityTypeBuilder<SingleGapQuestion> builder)
        {
            builder.ToTable("SingleGapQuestions");

            builder.HasMany(x => x.Answers)
                .WithOne(x => x.SingleGapQuestion)
                .HasForeignKey(x => x.SingleGapQuestionId);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId);
        }
    }
}
