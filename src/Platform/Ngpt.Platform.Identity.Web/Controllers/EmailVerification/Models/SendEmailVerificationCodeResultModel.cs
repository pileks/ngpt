using System;

namespace Ngpt.Platform.Identity.Web.Controllers.EmailVerification.Models
{
    public class SendEmailVerificationCodeResultModel
    {
        public bool MaximumAttemptsReached { get; set; }
        public DateTime DateWhenResendIsAvailable { get; set; }
    }
}