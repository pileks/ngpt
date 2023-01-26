using System.Collections.Generic;
using Augur.Emails.Models;

namespace Ngpt.Web.EmailTemplates.OrganizationUserInvitation
{
    public class OrganizationUserInvitationTemplate : EmailTemplateDefinition
    {
        public OrganizationUserInvitationTemplate(string applicationUrl, string logoUrl, string name, string email, string password, string orgName) : base(name: "OrganizationUserInvitation", subject: "Ngpt - Organization invitation", namespaceDefinition: "Ngpt.Web.EmailTemplates")
        {
            this.ApplicationUrl = applicationUrl;
            this.LogoUrl = logoUrl;
            this.UserAccountName = name;
            this.Email = email;
            this.Password = password;
            this.OrgName = orgName;
        }

        public string ApplicationUrl { get; }
        public string LogoUrl { get; }
        public string UserAccountName { get; }
        public string Email { get; }
        public string Password { get; }
        public string OrgName { get; }

        public override IDictionary<string, string> Tokens => new Dictionary<string, string>
        {
            { "{{applicationUrl}}", this.ApplicationUrl },
            { "{{name}}", this.UserAccountName },
            { "{{logoUrl}}", this.LogoUrl },
            { "{{email}}", this.Email },
            { "{{password}}", this.Password },
            { "{{orgName}}", this.OrgName },
        };
    }
}
