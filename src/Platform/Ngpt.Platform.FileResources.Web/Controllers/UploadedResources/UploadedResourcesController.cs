using System.IO;
using System.Threading.Tasks;
using Augur.Web;
using Augur.Web.Controllers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.UploadedResources;
using Ngpt.Platform.FileResources.Web.ApplicationSettings;
using Ngpt.Platform.Identity.Web.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ngpt.Platform.FileResources.Web.Controllers.UploadedResources
{
    public class UploadedResourcesController : AugurEntityController<UploadedResource>
    {
        private readonly UploadedResourcesSettings uploadedResourcesSettings;
        private readonly RootDbContext rootDbContext;

        public UploadedResourcesController(
            UploadedResourcesSettings uploadedResourcesSettings,
            RootDbContext rootDbContext)
            : base(rootDbContext)
        {
            this.uploadedResourcesSettings = uploadedResourcesSettings;
            this.rootDbContext = rootDbContext;
        }

        [RequireUserWithVerifiedEmailLoggedIn]
        [HttpGet(nameof(Download))]
        [ExportToFrontendWithResponseType("blob")]
        [ExportToFrontendWithCustomHeaders]
        public async Task<IActionResult> Download(int resourceId)
        {
            var resource = await this.rootDbContext.Set<UploadedResource>().FindAsync(resourceId);

            var fullPath = Path.Combine(this.uploadedResourcesSettings.Directory, $"{resource.Guid}-{resource.Name}");

            var bytes = System.IO.File.ReadAllBytes(fullPath);

            return this.File(bytes, resource.MimeType, resource.Name);
        }

        [RequireUserWithVerifiedEmailLoggedIn]
        public override async Task<IActionResult> Delete(int id)
        {
            var resource = await this.rootDbContext.Set<UploadedResource>().FindAsync(id);

            var fullPath = Path.Combine(this.uploadedResourcesSettings.Directory, $"{resource.Guid}-{resource.Name}");

            System.IO.File.Delete(fullPath);

            return await base.Delete(id);
        }

        [HttpGet(nameof(Preview))]
        public async Task<IActionResult> Preview(int resourceId)
        {
            var resource = await this.rootDbContext.Set<UploadedResource>().FindAsync(resourceId);

            var fullPath = Path.Combine(this.uploadedResourcesSettings.Directory, $"{resource.Guid}-{resource.Name}");

            var bytes = System.IO.File.ReadAllBytes(fullPath);

            return this.File(bytes, resource.MimeType, resource.Name);
        }
    }
}
