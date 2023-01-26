using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.SingleAnswer;

namespace Ngpt.Data.DbMaps.Questions.SingleAnswer
{
    public class SingleAnswerQuestionDbMap : IEntityTypeConfiguration<SingleAnswerQuestion>
    {
        public void Configure(EntityTypeBuilder<SingleAnswerQuestion> builder)
        {
            builder.ToTable("SingleAnswerQuestions");

            builder.HasMany(x => x.Answers)
                .WithOne(y => y.Question)
                .HasForeignKey(y => y.QuestionId);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId);
        }
    }
}
