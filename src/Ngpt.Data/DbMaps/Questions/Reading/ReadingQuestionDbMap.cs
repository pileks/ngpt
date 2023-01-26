using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.Reading;

namespace Ngpt.Data.DbMaps.Questions.Reading
{
    public class ReadingQuestionDbMap : IEntityTypeConfiguration<ReadingQuestion>
    {
        public void Configure(EntityTypeBuilder<ReadingQuestion> builder)
        {
            builder.ToTable("ReadingQuestions");

            builder.HasOne(x => x.Text)
                .WithMany()
                .HasForeignKey(x => x.TextId);

            builder.HasMany(x => x.Answers)
                .WithOne(y => y.Question)
                .HasForeignKey(x => x.QuestionId);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId);
        }
    }
}
