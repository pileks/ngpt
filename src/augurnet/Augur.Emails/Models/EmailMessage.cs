using System.Collections.Generic;
using SendGrid.Helpers.Mail;

namespace Augur.Emails.Models
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            this.Attachments = new List<EmailMessageAttachment>();
            this.Recipients = new List<EmailAddress>();
        }

        public string Subject { get; set; }
        public string PlainTextBody { get; set; }
        public string HtmlBody { get; set; }

        public EmailAddress Sender { get; set; }

        public List<EmailAddress> Recipients { get; set; }

        public List<EmailMessageAttachment> Attachments { get; set; }
        public bool ShowAllRecipients { get; set; }
    }
}