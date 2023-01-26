namespace Ngpt.Platform.Identity.Web.Models
{
    public class SignInResponseModel
    {
        public string Token { get; set; }
        public LoggedInUserInfoModel User { get; set; }
    }
}