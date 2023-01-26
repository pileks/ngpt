namespace Ngpt.Platform.Identity.Web.Controllers.EmailVerification.Models
{
    public class ConfirmEmailVerificationCodeResultModel
    {
        public bool HasErrors { get; set; }
        public EmailVerificationErrorType ErrorType { get; set; }
    }
}