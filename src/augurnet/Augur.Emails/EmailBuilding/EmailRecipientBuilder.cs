using System;
using System.Collections.Generic;
using Augur.Emails.Models;
using Augur.Emails.Settings;
using SendGrid.Helpers.Mail;

namespace Augur.Emails.EmailBuilding
{
    public class EmailRecipientBuilder
    {
        private readonly EmailSettings emailSettings;
        private readonly EmailMessage emailMessage;

        public EmailRecipientBuilder(EmailSettings emailSettings, EmailMessage emailMessage)
        {
            this.emailMessage = emailMessage;
            this.emailSettings = emailSettings;
        }

        public EmailBuildFinisher AddRecipient(string email, string name)
        {
            if (email == null)
            {
                throw new Exception("Recipient email cannot be null.");
            }

            if (name == null)
            {
                throw new Exception("Recipient name cannot be null.");
            }

            var recipientEmailAddress = new EmailAddress(email, name);

            var processedRecipientEmailAddress = this.RedirectEmailIfRedirectConfigured(recipientEmailAddress);

            this.emailMessage.Recipients.Add(processedRecipientEmailAddress);

            return new EmailBuildFinisher(this.emailSettings, this.emailMessage);
        }

        public EmailBuildFinisher AddRecipients(IEnumerable<(string Email, string Name)> emailNamePairs, bool showAllRecipients = false)
        {
            foreach (var pair in emailNamePairs)
            {
                this.AddRecipient(pair.Email, pair.Name);
            }

            this.emailMessage.ShowAllRecipients = showAllRecipients;

            return new EmailBuildFinisher(this.emailSettings, this.emailMessage);
        }

        private EmailAddress RedirectEmailIfRedirectConfigured(EmailAddress recipientEmail)
        {
            return string.IsNullOrWhiteSpace(this.emailSettings.RedirectSendGridToEmail)
                ? recipientEmail
                : new EmailAddress(this.emailSettings.RedirectSendGridToEmail, recipientEmail.Name);
        }
    }
}