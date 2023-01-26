using System.Collections.Generic;
using Augur.Emails;
using Augur.Emails.Models;

namespace Ngpt.Platform.Identity.Web.EmailTemplates.PasswordReset
{
    public class PasswordResetTemplate : EmailTemplateDefinition
    {
        public PasswordResetTemplate(string applicationUrl, string uid, string usersName, string logoUrl) 
            : base(name: "PasswordReset", subject: "Ngpt - Password reset", "Ngpt.Platform.Identity.Web.EmailTemplates")
        {
            this.ApplicationUrl = applicationUrl;
            this.Uid = uid;
            this.UsersName = usersName;
            this.LogoUrl = logoUrl;
        }

        public string ApplicationUrl { get; set; }
        public string Uid { get; set; }
        public string UsersName { get; set; }
        public string LogoUrl { get; set; }

        public override IDictionary<string, string> Tokens => new Dictionary<string, string>
        {
            { "{{usersName}}", this.UsersName },
            { "{{logoUrl}}", this.LogoUrl },
            { "{{applicationUrl}}", this.ApplicationUrl },
            { "{{uid}}", this.Uid }
        };
    }
}
