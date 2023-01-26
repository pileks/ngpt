using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuthToken = Ngpt.Platform.Data.Entities.AuthTokens.AuthToken;

namespace Ngpt.Platform.Data.Entities.AuthTokens
{
    public class AuthTokenDbMap : IEntityTypeConfiguration<AuthToken>
    {
        public AuthTokenDbMap()
        {
        }

        public void Configure(EntityTypeBuilder<AuthToken> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.ToTable("AuthTokens");
        }
    }
}
