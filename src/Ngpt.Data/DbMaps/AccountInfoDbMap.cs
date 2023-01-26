using Ngpt.Platform.Data.Entities.AccountInfos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ngpt.Data.DbMaps
{
    public class AccountInfoDbMap : IEntityTypeConfiguration<AccountInfo>
    {
        public void Configure(EntityTypeBuilder<AccountInfo> builder)
        {
            builder.ToTable("AccountInfos");

            builder.HasOne(x => x.User)
                .WithOne(u => u.AccountInfo)
                .HasForeignKey<AccountInfo>(x => x.UserId);
        }
    }
}