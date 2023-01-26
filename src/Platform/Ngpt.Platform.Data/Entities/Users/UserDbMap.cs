using Ngpt.Platform.Data.Entities.AccountInfos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ngpt.Platform.Data.Entities.Users
{
    public class UserDbMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasOne(x => x.AccountInfo)
                .WithOne(a => a.User)
                .HasForeignKey<AccountInfo>(x => x.UserId);

            builder.Ignore(x => x.UserId);

            builder.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId);
        }
    }
}
