using System;
using System.Collections.Generic;
using Augur.Emails.Models;
using Augur.Emails.Settings;

namespace Augur.Emails.EmailBuilding
{
    public class EmailAttachmentBuilder
    {
        private readonly EmailSettings emailSettings;
        private readonly EmailMessage emailMessage;

        public EmailAttachmentBuilder(EmailSettings emailSettings, EmailMessage emailMessage)
        {
            this.emailSettings = emailSettings;
            this.emailMessage = emailMessage;
        }

        public EmailFromBuilder AddAttachment(byte[] attachmentBytes, string fileName)
        {
            if (attachmentBytes == null)
            {
                throw new Exception("Attachment data cannot be null.");
            }

            if (fileName == null)
            {
                throw new Exception("Attachment file name cannot be null.");
            }

            this.emailMessage.Attachments.Add(new EmailMessageAttachment
            {
                AttachmentBytes = attachmentBytes,
                FileName = fileName
            });

            return new EmailFromBuilder(this.emailSettings, this.emailMessage);
        }

        public EmailFromBuilder AddAttachments(IEnumerable<(byte[] AttachmentBytes, string FileName)> dataFileNamePairs)
        {

            foreach (var pair in dataFileNamePairs)
            {
                this.emailMessage.Attachments.Add(new EmailMessageAttachment
                {
                    AttachmentBytes = pair.AttachmentBytes,
                    FileName = pair.FileName
                });
            }

            return new EmailFromBuilder(this.emailSettings, this.emailMessage);
        }

        public EmailFromBuilder NoAttachments()
        {
            this.emailMessage.Attachments.Clear();

            return new EmailFromBuilder(this.emailSettings, this.emailMessage);
        }
    }
}