using Augur.Data.DbMaps.Shared;
using Augur.Data.Extensions;
using Augur.Data.Interfaces.Providers;
using Augur.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Augur.Data.DbMaps
{
    //public class AugurUserRoleDbMap : TenantEntityDbMap<AugurUserRole>
    //{
    //    public AugurUserRoleDbMap(IAugurLoggedInTenantIdProvider tenantIdProvider) : base(tenantIdProvider)
    //    {
    //    }

    //    public override void Configure(EntityTypeBuilder<AugurUserRole> builder)
    //    {
    //        base.Configure(builder);

    //        builder.HasKey(a => new { a.AugurUserId, a.AugurRoleId });

    //        builder.HasOne(a => a.AugurUser)
    //            .WithMany("AugurUserRoles")
    //            .HasForeignKey(a => a.AugurUserId);

    //        builder.HasOne(a => a.AugurRole)
    //            .WithMany("AugurUserRoles")
    //            .HasForeignKey(a => a.AugurRoleId);

    //        builder.BuildDefaultPostgresTableColumnsConfiguration();

    //        builder.ToTable("augur_user_roles");
    //    }
    //}

    //public class TenantAgnosticAugurUserRoleDbMap : IEntityTypeConfiguration<AugurUserRole>
    //{
    //    public void Configure(EntityTypeBuilder<AugurUserRole> builder)
    //    {
    //        builder.HasKey(a => new { a.AugurUserId, a.AugurRoleId });

    //        builder.HasOne(a => a.AugurUser)
    //            .WithMany("AugurUserRoles")
    //            .HasForeignKey(a => a.AugurUserId);

    //        builder.HasOne(a => a.AugurRole)
    //            .WithMany("AugurUserRoles")
    //            .HasForeignKey(a => a.AugurRoleId);

    //        builder.BuildDefaultPostgresTableColumnsConfiguration();

    //        builder.ToTable("augur_user_roles");
    //    }
    //}
}