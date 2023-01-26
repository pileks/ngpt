using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ngpt.Platform.Data.Entities.PermissionOverrides
{
    public class PermissionOverrideDbMap : IEntityTypeConfiguration<PermissionOverride>
    {
        public PermissionOverrideDbMap()
        {
        }

        public void Configure(EntityTypeBuilder<PermissionOverride> builder)
        {
            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);

            builder.ToTable("PermissionOverrides");
        }
    }
}
