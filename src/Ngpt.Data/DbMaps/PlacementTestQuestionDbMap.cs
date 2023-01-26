using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities;

namespace Ngpt.Data.DbMaps
{
    public class PlacementTestQuestionDbMap : IEntityTypeConfiguration<PlacementTestQuestion>
    {
        public void Configure(EntityTypeBuilder<PlacementTestQuestion> builder)
        {
            builder.ToTable("PlacementTestQuestions");

            builder.HasOne(x => x.Question)
                .WithMany()
                .HasForeignKey(x => x.QuestionId);

            builder.HasOne(x => x.PlacementTest)
                .WithMany()
                .HasForeignKey(x => x.PlacementTestId);
        }
    }
}