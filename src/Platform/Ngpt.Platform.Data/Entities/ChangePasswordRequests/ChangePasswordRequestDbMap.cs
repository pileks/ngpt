using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ngpt.Platform.Data.Entities.ChangePasswordRequests
{
    public class ChangePasswordRequestDbMap : IEntityTypeConfiguration<ChangePasswordRequest>
    {
        public void Configure(EntityTypeBuilder<ChangePasswordRequest> builder)
        {
            builder.ToTable("ChangePasswordRequests");

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.SentEmail)
                .WithMany()
                .HasForeignKey(x => x.SentEmailId);
        }
    }
}
