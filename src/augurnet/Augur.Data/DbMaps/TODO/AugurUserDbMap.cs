using Augur.Data.DbMaps.Shared;
using Augur.Data.Interfaces.Providers;
using Augur.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Augur.Data.DbMaps
{
    //public class AugurTenantUserDbMap : TenantEntityDbMap<AugurTenantUser>
    //{
    //    public AugurTenantUserDbMap(IAugurLoggedInTenantIdProvider tenantIdProvider) : base(tenantIdProvider)
    //    {
    //    }

    //    public override void Configure(EntityTypeBuilder<AugurTenantUser> builder)
    //    {
    //        base.Configure(builder);

    //        builder.HasOne(u => u.Tenant)
    //            .WithMany(t => t.AugurTenantUsers)
    //            .HasForeignKey(u => u.TenantId);

    //        builder.ToTable("AugurTenantUsers");
    //    }
    //}

    //public class TenantAgnosticAugurUserDbMap : IEntityTypeConfiguration<AugurTenantUser>
    //{
    //    public TenantAgnosticAugurUserDbMap()
    //    {
    //    }

    //    public void Configure(EntityTypeBuilder<AugurTenantUser> builder)
    //    {
    //        builder.HasOne(u => u.Tenant)
    //            .WithMany(t => t.AugurTenantUsers)
    //            .HasForeignKey(u => u.TenantId);

    //        builder.ToTable("AugurTenantUsers");
    //    }
    //}
}