using System;

namespace Augur.Emails.Models
{
    public class SentEmail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string PlainTextBody { get; set; }
        public DateTime Date { get; set; }
    }
}
