using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.Listening;

namespace Ngpt.Data.DbMaps.Questions.Listening
{
    public class ListeningQuestionDbMap : IEntityTypeConfiguration<ListeningQuestion>
    {
        public void Configure(EntityTypeBuilder<ListeningQuestion> builder)
        {
            builder.ToTable("ListeningQuestions");

            builder.HasOne(x => x.Audio)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.AudioId);

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
