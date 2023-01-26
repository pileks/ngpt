using Augur.Emails.Settings;
using Ngpt.Platform.FileResources.Web.ApplicationSettings;
using Ngpt.Platform.Identity.Web.Settings;

namespace Ngpt.Web.ApplicationSettings
{
    public class AppSettings
    {
        public ConnectionStringsSettings ConnectionStrings { get; set; }
        public EmailSettings Email { get; set; }
        public EmailVerificationSettings EmailVerification { get; set; }
        public UploadedResourcesSettings UploadedResources { get; set; }
        public AuthenticationSettings Authentication { get; set; }
    }
}
