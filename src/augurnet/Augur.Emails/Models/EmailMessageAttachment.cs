namespace Augur.Emails.Models
{
    public class EmailMessageAttachment
    {
        public byte[] AttachmentBytes { get; set; }
        public string FileName { get; set; }
    }
}