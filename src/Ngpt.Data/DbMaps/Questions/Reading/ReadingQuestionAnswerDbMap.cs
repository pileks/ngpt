using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.Reading;

namespace Ngpt.Data.DbMaps.Questions.Reading
{
    public class ReadingQuestionAnswerDbMap : IEntityTypeConfiguration<ReadingQuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<ReadingQuestionAnswer> builder)
        {
            builder.ToTable("ReadingQuestionAnswers");

            builder.HasOne(x => x.Question)
                .WithMany(y => y.Answers)
                .HasForeignKey(x => x.QuestionId);

            builder.HasOne(x => x.Image)
                .WithMany()
                .HasForeignKey(x => x.ImageId);
        }
    }
}
