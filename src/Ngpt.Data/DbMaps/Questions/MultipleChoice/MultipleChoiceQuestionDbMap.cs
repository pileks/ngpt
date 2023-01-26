using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.MultipleChoice;

namespace Ngpt.Data.DbMaps.Questions.MultipleChoice
{
    public class MultipleChoiceQuestionDbMap : IEntityTypeConfiguration<MultipleChoiceQuestion>
    {
        public void Configure(EntityTypeBuilder<MultipleChoiceQuestion> builder)
        {
            builder.ToTable("MultipleChoiceQuestions");

            builder.HasMany(x => x.Parts)
                .WithOne(y => y.Question)
                .HasForeignKey(y=> y.QuestionId);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId);
        }
    }
}
