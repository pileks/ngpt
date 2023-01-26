using Augur.Emails;
using System.Collections.Generic;
using Augur.Emails.Models;

namespace Ngpt.Platform.Identity.Web.EmailTemplates.EmailConfirmation
{
    public class EmailConfirmationTemplate : EmailTemplateDefinition
    {
        public EmailConfirmationTemplate(string code, string usersName, string logoUrl) 
            : base(name: "EmailConfirmation", subject: "Ngpt - Email verification", "Ngpt.Platform.Identity.Web.EmailTemplates")
        {
            this.Code = code;
            this.UsersName = usersName;
            this.LogoUrl = logoUrl;
        }

        public string Code { get; set; }
        public string UsersName { get; set; }
        public string LogoUrl { get; set; }

        public override IDictionary<string, string> Tokens => new Dictionary<string, string>
        {
            { "{{logoUrl}}", this.LogoUrl },
            { "{{emailVerificationCode}}", this.Code },
            { "{{usersName}}", this.UsersName }
        };
    }
}
