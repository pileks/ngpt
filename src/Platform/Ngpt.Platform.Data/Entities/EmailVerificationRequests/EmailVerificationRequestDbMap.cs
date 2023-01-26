using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ngpt.Platform.Data.Entities.EmailVerificationRequests
{
    public class EmailVerificationRequestDbMap : IEntityTypeConfiguration<EmailVerificationRequest>
    {
        public void Configure(EntityTypeBuilder<EmailVerificationRequest> builder)
        {
            builder.ToTable("EmailVerificationRequests");

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.SentEmail)
                .WithMany()
                .HasForeignKey(x => x.SentEmailId);
        }
    }
}
