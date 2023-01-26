namespace Augur.Emails.Settings
{
    public class EmailSettings
    {
        public bool IsEnabled { get; set; }
        public string RedirectSendGridToEmail { get; set; }
        public string AdminEmail { get; set; }
        public string AdminDisplayName { get; set; }
        public string SendGridApiKey { get; set; }
        public string LogoUrl { get; set; }
    }
}