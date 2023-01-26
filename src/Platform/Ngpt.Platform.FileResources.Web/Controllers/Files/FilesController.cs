using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Augur.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.UploadedResources;
using Ngpt.Platform.FileResources.Web.ApplicationSettings;
using Ngpt.Platform.Identity.Web.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace Ngpt.Platform.FileResources.Web.Controllers.Files
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class FilesController : AugurApiController
    {
        private readonly RootDbContext rootDbContext;
        private readonly UploadedResourcesSettings uploadedResourcesSettings;

        public FilesController(RootDbContext rootDbContext, UploadedResourcesSettings uploadedResourcesSettings)
        {
            this.rootDbContext = rootDbContext;
            this.uploadedResourcesSettings = uploadedResourcesSettings;
        }

        [HttpPost(nameof(Upload)), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            var file = this.Request.Form.Files[0];

            return await UploadFileAndCreateResource(file);
        }

        [HttpPost(nameof(UploadImageAndResize)), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadImageAndResize()
        {
            var file = this.Request.Form.Files[0];

            if (!file.FileName.ToLowerInvariant().EndsWith(".jpg") &&
                !file.FileName.ToLowerInvariant().EndsWith(".jpeg") &&
                !file.FileName.ToLowerInvariant().EndsWith(".png"))
            {
                return this.BadRequest("File must be of type jpg, jpeg or png!");
            }

            if (file.Length > 0)
            {
                var stream = new MemoryStream();
                await file.CopyToAsync(stream);

                stream.Seek(0, SeekOrigin.Begin);

                var image = await Image.LoadAsync(stream);

                image.Mutate(x =>
                    x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(1000, 1000),
                        Sampler = new BicubicResampler()
                    }));

                var guid = Guid.NewGuid().ToString();

                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(this.uploadedResourcesSettings.Directory, $"{guid}-{fileName}");

                await image.SaveAsync(fullPath);

                var resource = new UploadedResource
                {
                    Name = fileName,
                    Guid = guid,
                    MimeType = file.ContentType,
                    CreatedOn = DateTime.UtcNow
                };

                this.rootDbContext.Set<UploadedResource>().Add(resource);

                await this.rootDbContext.SaveChangesAsync();

                return this.Ok(new { resourceId = resource.Id });
            }

            return this.BadRequest("File size must be larger than zero!");
        }

        [HttpPost(nameof(UploadAudio)), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAudio()
        {
            var file = this.Request.Form.Files[0];

            if (!file.FileName.ToLowerInvariant().EndsWith(".mp3") &&
                !file.FileName.ToLowerInvariant().EndsWith(".ogg") &&
                !file.FileName.ToLowerInvariant().EndsWith(".wav"))
            {
                return this.BadRequest("File must be of type mp3, ogg or wav!");
            }

            return await UploadFileAndCreateResource(file);
        }

        private async Task<IActionResult> UploadFileAndCreateResource(IFormFile file)
        {
            if (file.Length > 0)
            {
                var guid = Guid.NewGuid().ToString();

                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(this.uploadedResourcesSettings.Directory, $"{guid}-{fileName}");

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var resource = new UploadedResource
                {
                    Name = fileName,
                    Guid = guid,
                    MimeType = file.ContentType,
                    CreatedOn = DateTime.UtcNow
                };

                this.rootDbContext.Set<UploadedResource>().Add(resource);

                await this.rootDbContext.SaveChangesAsync();

                return this.Ok(new { resourceId = resource.Id });
            }

            return this.BadRequest("File size must be larger than zero!");
        }
    }
}
