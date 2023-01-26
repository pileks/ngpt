using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Augur.Emails.Models;
using Augur.Emails.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Augur.Emails.Services
{
    public class EmailSender
    {
        private readonly EmailSettings emailSettings;
        private readonly EmailMessage emailMessage;

        public EmailSender(EmailSettings emailSettings, EmailMessage emailMessage)
        {
            this.emailSettings = emailSettings;
            this.emailMessage = emailMessage;
        }
        
        public async Task<IList<SentEmail>> Send()
        {
            SendGridMessage sendGridMessage = null;

            if (this.emailMessage.Recipients.Count <= 1)
            {
                sendGridMessage = MailHelper.CreateSingleEmail(
                    this.emailMessage.Sender,
                    this.emailMessage.Recipients.First(),
                    this.emailMessage.Subject,
                    this.emailMessage.PlainTextBody,
                    this.emailMessage.HtmlBody);
            }
            else
            {
                sendGridMessage = MailHelper.CreateSingleEmailToMultipleRecipients(
                    this.emailMessage.Sender,
                    this.emailMessage.Recipients,
                    this.emailMessage.Subject,
                    this.emailMessage.PlainTextBody,
                    this.emailMessage.HtmlBody,
                    this.emailMessage.ShowAllRecipients);
            }


            foreach (var attachment in this.emailMessage.Attachments)
            {
                sendGridMessage.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.AttachmentBytes));
            }

            if (this.emailSettings.IsEnabled)
            {
                var response = await new SendGridClient(this.emailSettings.SendGridApiKey).SendEmailAsync(sendGridMessage);

                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    throw new Exception("Failed sending email: " + response.Body.ReadAsStringAsync().Result);
                }
            }

            return this.emailMessage.Recipients.Select(recipient => new SentEmail
            {
                From = this.emailMessage.Sender.Email,
                To = recipient.Email,
                Subject = this.emailMessage.Subject,
                HtmlBody = this.emailMessage.HtmlBody,
                PlainTextBody = this.emailMessage.PlainTextBody,
                Date = DateTime.UtcNow
            }).ToList();
        }

        //private SendGridMessage CreateMultipleEmailsToMultipleRecipientsMessage(List<EmailAddress> recipientEmails, List<string> subjects, string plainTextBody, string htmlBody, List<Dictionary<string, string>> substitutions)
        //{
        //    recipientEmails = this.RedirectEmailIfRedirectConfigured(recipientEmails);

        //    var from = new EmailAddress(this.tenantDbContext.LoggedInUser.Tenant.AdministratorEmail, this.tenantDbContext.LoggedInUser.Tenant.DisplayName);

        //    return MailHelper.CreateMultipleEmailsToMultipleRecipients(
        //        from,
        //        recipientEmails,
        //        subjects,
        //        plainTextBody,
        //        htmlBody,
        //        substitutions);
        //}
    }
}