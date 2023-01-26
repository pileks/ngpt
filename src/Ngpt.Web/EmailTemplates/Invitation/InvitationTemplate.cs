using Augur.Emails.Models;
using System.Collections.Generic;

namespace Ngpt.Web.EmailTemplates.Invitation
{
    public class InvitationTemplate : EmailTemplateDefinition
    {
        public InvitationTemplate(string applicationUrl, string token, string logoUrl, string projectCoordinator, string projectTitle) : base(name: "Invitation", subject: "Ngpt - Project invitation", namespaceDefinition: "Ngpt.Web.EmailTemplates")
        {
            ApplicationUrl = applicationUrl;
            Token = token;
            LogoUrl = logoUrl;
            ProjectCoordinator = projectCoordinator;
            ProjectTitle = projectTitle;
        }

        public string ApplicationUrl { get; }
        public string Token { get; }
        public string LogoUrl { get; }
        public string ProjectCoordinator { get; }
        public string ProjectTitle { get; }

        public override IDictionary<string, string> Tokens => new Dictionary<string, string>
        {
            { "{{applicationUrl}}", this.ApplicationUrl },
            { "{{token}}", this.Token },
            { "{{logoUrl}}", this.LogoUrl },
            { "{{projectCoordinator}}", this.ProjectCoordinator },
            { "{{projectTitle}}", this.ProjectTitle },
        };
    }
}
