using System;
using Augur.Entity.Base.Entities;

namespace Ngpt.Platform.Data.Entities.UploadedResources
{
    public partial class UploadedResource : AugurEntityWithId
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public string MimeType { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsInUse { get; set; }
    }
}