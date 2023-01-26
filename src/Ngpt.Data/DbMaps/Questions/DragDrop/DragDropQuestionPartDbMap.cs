using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.DragDrop;

namespace Ngpt.Data.DbMaps.Questions.DragDrop
{
    public class DragDropQuestionPartDbMap : IEntityTypeConfiguration<DragDropQuestionPart>
    {
        public void Configure(EntityTypeBuilder<DragDropQuestionPart> builder)
        {
            builder.ToTable("DragDropQuestionParts");

            builder.HasOne(x => x.Question)
                .WithMany(x => x.Parts)
                .HasForeignKey(x => x.QuestionId);
        }
    }
}