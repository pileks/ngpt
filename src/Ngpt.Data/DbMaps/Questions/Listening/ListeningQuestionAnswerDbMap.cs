using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.Listening;

namespace Ngpt.Data.DbMaps.Questions.Listening
{
    public class ListeningQuestionAnswerDbMap : IEntityTypeConfiguration<ListeningQuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<ListeningQuestionAnswer> builder)
        {
            builder.ToTable("ListeningQuestionAnswers");

            builder.HasOne(x => x.Image)
                .WithMany()
                .HasForeignKey(x => x.ImageId);

            builder.HasOne(x => x.Question)
                .WithMany(y => y.Answers)
                .HasForeignKey(x => x.QuestionId);
        }
    }
}
