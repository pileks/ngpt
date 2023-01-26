using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ngpt.Data.Entities;

namespace Ngpt.Data.DbMaps
{
    public class PlacementTestInvitationDbMap : IEntityTypeConfiguration<PlacementTestInvitation>
    {
        public void Configure(EntityTypeBuilder<PlacementTestInvitation> builder)
        {
            builder.ToTable("PlacementTestInvitations");

            builder.HasOne(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.LanguageId);

            builder.HasOne(x => x.PlacementTest)
                .WithMany()
                .HasForeignKey(x => x.PlacementTestId);
        }
    }
}