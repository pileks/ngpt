using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions;

namespace Ngpt.Data.DbMaps.Questions
{
    public class UseOfLanguageQuestionDbMap : IEntityTypeConfiguration<UseOfLanguageQuestion>
    {
        public void Configure(EntityTypeBuilder<UseOfLanguageQuestion> builder)
        {
            builder.ToTable("UseOfLanguageQuestions");

            builder.HasOne(x => x.MultipleChoiceQuestion)
                .WithMany()
                .HasForeignKey(x => x.MultipleChoiceQuestionId);

            builder.HasOne(x => x.SingleGapQuestion)
                .WithMany()
                .HasForeignKey(x => x.SingleGapQuestionId);

            builder.HasOne(x => x.DragDropQuestion)
                .WithMany()
                .HasForeignKey(x => x.DragDropQuestionId);

            builder.HasOne(x => x.SingleAnswerQuestion)
                .WithMany()
                .HasForeignKey(x => x.SingleAnswerQuestionId);

            builder.HasOne(x => x.Instruction)
                .WithMany()
                .HasForeignKey(x => x.InstructionId);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId);

            builder.HasOne(x => x.Approver)
                .WithMany()
                .HasForeignKey(x => x.ApproverId);
        }
    }
}
