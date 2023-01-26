using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ngpt.Platform.Data.Entities.UploadedResources
{
    public class UploadedResourceDbMap : IEntityTypeConfiguration<UploadedResource>
    {
        public void Configure(EntityTypeBuilder<UploadedResource> builder)
        {
            builder.ToTable("UploadedResources");
        }
    }
}
