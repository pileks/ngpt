namespace Ngpt.Platform.Identity.Web.Controllers.Registration.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string TenantName { get; set; }
        public string Name { get; set; }

        public bool HasAcceptedTermsAndPrivacyPolicy { get; set; }
    }
}
