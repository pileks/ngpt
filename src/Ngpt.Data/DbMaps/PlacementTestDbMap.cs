using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities;

namespace Ngpt.Data.DbMaps
{
    public class PlacementTestDbMap : IEntityTypeConfiguration<PlacementTest>
    {
        public void Configure(EntityTypeBuilder<PlacementTest> builder)
        {
            builder.ToTable("PlacementTests");

            builder.HasOne(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.LanguageId);

            builder.HasOne(x => x.ReportedLevel)
                .WithMany()
                .HasForeignKey(x => x.ReportedLevelId);

            builder.HasOne(x => x.ReadingQuestionText)
                .WithMany()
                .HasForeignKey(x => x.ReadingQuestionTextId);

            builder.HasOne(x => x.ListeningQuestionAudio)
                .WithMany()
                .HasForeignKey(x => x.ListeningQuestionAudioId);
        }
    }
}