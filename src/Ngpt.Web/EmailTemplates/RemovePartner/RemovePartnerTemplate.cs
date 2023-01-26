using Augur.Emails.Models;
using System.Collections.Generic;

namespace Ngpt.Web.EmailTemplates.RemovePartner
{
    public class RemovePartnerTemplate : EmailTemplateDefinition
    {
        public RemovePartnerTemplate(string applicationUrl, string logoUrl, string projectCoordinator, string projectTitle) : base(name: "RemovePartner", subject: "Ngpt - You have been removed from a project", namespaceDefinition: "Ngpt.Web.EmailTemplates")
        {
            ApplicationUrl = applicationUrl;
            LogoUrl = logoUrl;
            ProjectCoordinator = projectCoordinator;
            ProjectTitle = projectTitle;
        }

        public string ApplicationUrl { get; }
        public string LogoUrl { get; }
        public string ProjectCoordinator { get; }
        public string ProjectTitle { get; }

        public override IDictionary<string, string> Tokens => new Dictionary<string, string>
        {
            { "{{applicationUrl}}", this.ApplicationUrl },
            { "{{logoUrl}}", this.LogoUrl },
            { "{{projectCoordinator}}", this.ProjectCoordinator },
            { "{{projectTitle}}", this.ProjectTitle },
        };
    }
}
