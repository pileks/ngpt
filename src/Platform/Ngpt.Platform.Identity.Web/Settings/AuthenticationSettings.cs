namespace Ngpt.Platform.Identity.Web.Settings
{
    public class AuthenticationSettings
    {
        public double? TokenExpirationHours { get; set; }
        public bool EnableMultipleDeviceLogin { get; set; }
        public bool EnableExtendTokenExpiration { get; set; }

        public static double DefaultTokenExpirationHours = 24;
    }
}