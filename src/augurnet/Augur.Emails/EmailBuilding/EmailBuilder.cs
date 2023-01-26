using System;
using Augur.Emails.Models;
using Augur.Emails.Settings;

namespace Augur.Emails.EmailBuilding
{
    public class EmailBuilder
    {
        private readonly EmailSettings emailSettings;
      
        public EmailBuilder(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public EmailAttachmentBuilder CreateEmail(string subject, string plainTextBody, string htmlBody)
        {
            if (subject == null)
            {
                throw new Exception("Subject cannot be null.");
            }

            if (plainTextBody == null)
            {
                throw new Exception("PlainTextBody cannot be null.");
            }

            if (htmlBody == null)
            {
                throw new Exception("HtmlBody cannot be null.");
            }

            var emailMessage = new EmailMessage
            {
                Subject = subject,
                PlainTextBody = plainTextBody,
                HtmlBody = htmlBody
            };

            return new EmailAttachmentBuilder(this.emailSettings, emailMessage);
        }
    }
}