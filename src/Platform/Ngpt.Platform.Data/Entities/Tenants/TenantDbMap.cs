using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ngpt.Platform.Data.Entities.Tenants
{
    public class TenantDbMap : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");

            builder.Ignore(x => x.TenantId);

            builder.HasOne(x => x.OrgOwner)
                .WithMany()
                .HasForeignKey(x => x.OrgOwnerId);
        }
    }
}
