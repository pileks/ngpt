namespace Ngpt.Platform.Identity.Web.Settings
{
    public class EmailVerificationSettings
    {
        public int MaxNumberOfAttempts { get; set; }
        public int ResendEmailTimeOutInMinutes { get; set; }
        public int VerificationCodeLength { get; set; }
    }
}