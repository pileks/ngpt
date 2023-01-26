using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities.Questions.Listening;

namespace Ngpt.Data.DbMaps.Questions.Listening
{
    public class ListeningQuestionAudioDbMap : IEntityTypeConfiguration<ListeningQuestionAudio>
    {
        public void Configure(EntityTypeBuilder<ListeningQuestionAudio> builder)
        {
            builder.ToTable("ListeningQuestionAudios");

            builder.HasOne(x => x.Resource)
                .WithMany()
                .HasForeignKey(x => x.ResourceId);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId);

            builder.HasMany(x => x.Questions)
                .WithOne(x => x.Audio)
                .HasForeignKey(x => x.AudioId);

            builder.HasOne(x => x.Approver)
                .WithMany()
                .HasForeignKey(x => x.ApproverId);
        }
    }
}
