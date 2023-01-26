using System;
using Augur.Emails.Models;
using Augur.Emails.Settings;
using SendGrid.Helpers.Mail;

namespace Augur.Emails.EmailBuilding
{
    public class EmailFromBuilder
    {
        private readonly EmailSettings emailSettings;
        private readonly EmailMessage emailMessage;

        public EmailFromBuilder(EmailSettings emailSettings, EmailMessage emailMessage)
        {
            this.emailMessage = emailMessage;
            this.emailSettings = emailSettings;
        }

        public EmailRecipientBuilder FromSender(string email, string name)
        {
            if (email == null)
            {
                throw new Exception("Sender email cannot be null.");
            }

            if (name == null)
            {
                throw new Exception("Sender name cannot be null.");
            }

            var from = new EmailAddress(email, name);

            this.emailMessage.Sender = from;

            return new EmailRecipientBuilder(this.emailSettings, this.emailMessage);
        }

        public EmailRecipientBuilder FromSystemAdmin()
        {
            return this.FromSender(this.emailSettings.AdminEmail, this.emailSettings.AdminDisplayName);
        }
    }
}