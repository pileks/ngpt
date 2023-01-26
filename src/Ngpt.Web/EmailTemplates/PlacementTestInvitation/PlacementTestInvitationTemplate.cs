using System.Collections.Generic;
using Augur.Emails.Models;

namespace Ngpt.Web.EmailTemplates.OrganizationUserInvitation
{
    public class PlacementTestInvitationTemplate : EmailTemplateDefinition
    {
        public PlacementTestInvitationTemplate(
            string applicationUrl, 
            string logoUrl, 
            string token) 
        : base(
              name: "PlacementTestInvitation", 
              subject: "NGPT - Take your placement test", 
              namespaceDefinition: "Ngpt.Web.EmailTemplates"
          )
        {
            this.ApplicationUrl = applicationUrl;
            this.LogoUrl = logoUrl;
            this.Token = token;
        }

        public string ApplicationUrl { get; }
        public string LogoUrl { get; }
        public string Token { get; }
        public string Email { get; }
        public string Password { get; }
        public string OrgName { get; }

        public override IDictionary<string, string> Tokens => new Dictionary<string, string>
        {
            { "{{applicationUrl}}", this.ApplicationUrl },
            { "{{token}}", this.Token },
            { "{{logoUrl}}", this.LogoUrl },
        };
    }
}
