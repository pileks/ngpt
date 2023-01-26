using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.Reading;

namespace Ngpt.Data.DbMaps.Questions.Reading
{
    public class ReadingQuestionTextDbMap : IEntityTypeConfiguration<ReadingQuestionText>
    {
        public void Configure(EntityTypeBuilder<ReadingQuestionText> builder)
        {
            builder.ToTable("ReadingQuestionTexts");

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId);

            builder.HasOne(x => x.Level)
                .WithMany()
                .HasForeignKey(x => x.LevelId);

            builder.HasOne(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.LanguageId);

            builder.HasMany(x => x.Questions)
                .WithOne(x => x.Text)
                .HasForeignKey(x => x.TextId);

            builder.HasOne(x => x.Approver)
                .WithMany()
                .HasForeignKey(x => x.ApproverId);
        }
    }
}
