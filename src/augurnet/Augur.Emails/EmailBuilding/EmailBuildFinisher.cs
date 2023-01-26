using Augur.Emails.Models;
using Augur.Emails.Services;
using Augur.Emails.Settings;

namespace Augur.Emails.EmailBuilding
{
    public class EmailBuildFinisher
    {
        private readonly EmailSettings emailSettings;
        private readonly EmailMessage emailMessage;

        public EmailBuildFinisher(EmailSettings emailSettings, EmailMessage emailMessage)
        {
            this.emailSettings = emailSettings;
            this.emailMessage = emailMessage;
        }

        public EmailSender Build()
        {
            return new EmailSender(this.emailSettings, this.emailMessage);
        }
    }
}