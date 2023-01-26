using Augur.Data.DbMaps.Shared;
using Augur.Data.Extensions;
using Augur.Data.Interfaces.Providers;
using Augur.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Augur.Data.DbMaps
{
    //public class AugurRoleDbMap : TenantEntityDbMap<AugurRole>
    //{
    //    public AugurRoleDbMap(IAugurLoggedInTenantIdProvider tenantIdProvider) : base(tenantIdProvider)
    //    {
    //    }

    //    public override void Configure(EntityTypeBuilder<AugurRole> builder)
    //    {
    //        base.Configure(builder);

    //        builder.BuildDefaultPostgresTableColumnsConfiguration();

    //        builder.ToTable("augur_roles");
    //    }
    //}
}