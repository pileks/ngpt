using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.DragDrop;

namespace Ngpt.Data.DbMaps.Questions.DragDrop
{
    public class DragDropQuestionDbMap : IEntityTypeConfiguration<DragDropQuestion>
    {
        public void Configure(EntityTypeBuilder<DragDropQuestion> builder)
        {
            builder.ToTable("DragDropQuestions");

            builder.HasMany(x => x.Parts)
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
