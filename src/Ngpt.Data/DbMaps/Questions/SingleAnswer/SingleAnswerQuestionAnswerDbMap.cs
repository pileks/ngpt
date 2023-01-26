using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.SingleAnswer;

namespace Ngpt.Data.DbMaps.Questions.SingleAnswer
{
    public class SingleAnswerQuestionAnswerDbMap : IEntityTypeConfiguration<SingleAnswerQuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<SingleAnswerQuestionAnswer> builder)
        {
            builder.ToTable("SingleAnswerQuestionAnswers");
            
            builder.HasOne(x => x.Image)
                .WithMany()
                .HasForeignKey(x => x.ImageId);

            builder.HasOne(x => x.Question)
                .WithMany(y => y.Answers)
                .HasForeignKey(x => x.QuestionId);
        }
    }
}
