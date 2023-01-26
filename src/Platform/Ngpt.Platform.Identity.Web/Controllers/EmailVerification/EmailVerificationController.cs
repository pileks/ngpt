using System;
using System.Linq;
using System.Threading.Tasks;
using Augur.ApiSystemEvents;
using Augur.Data.Extensions;
using Augur.Emails.Settings;
using Augur.Web;
using Augur.Web.Controllers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Data.Entities.EmailVerificationRequests;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Identity.Web.Controllers.EmailVerification.Models;
using Ngpt.Platform.Identity.Web.EmailTemplates.EmailConfirmation;
using Ngpt.Platform.Identity.Web.Settings;
using Ngpt.Platform.Services;
using Ngpt.Platform.Services.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ngpt.Platform.Identity.Web.Controllers.EmailVerification
{

    public class EmailVerificationController : AugurApiController
    {
        private readonly ILoggedInUserProvider loggedInUserProvider;
        private readonly RootDbContext rootDbContext;
        private readonly UserOwnedDbContext userOwnedDbContext;
        private readonly EmailSender<RootDbContext> emailSender;
        private readonly IHeaderClientNotifier headerClientNotifier;
        private readonly EmailSettings emailSettings;
        private readonly EmailVerificationSettings emailVerificationSettings;

        public EmailVerificationController(ILoggedInUserProvider loggedInUserProvider, RootDbContext rootDbContext, 
            UserOwnedDbContext userOwnedDbContext, EmailSender<RootDbContext> emailSender, IHeaderClientNotifier clientSystemEventNotifier, 
            IConfiguration configuration, EmailSettings emailSettings, EmailVerificationSettings emailVerificationSettings)
        {
            this.loggedInUserProvider = loggedInUserProvider;
            this.rootDbContext = rootDbContext;
            this.userOwnedDbContext = userOwnedDbContext;
            this.emailSender = emailSender;
            this.headerClientNotifier = clientSystemEventNotifier;
            this.emailSettings = emailSettings;
            this.emailVerificationSettings = emailVerificationSettings;
        }

        [RequireUserLoggedIn]
        [ExportToFrontendWithCustomHeaders]
        [HttpPost(nameof(ConfirmEmailVerificationCode))]
        public async Task<ActionResult<ConfirmEmailVerificationCodeResultModel>> ConfirmEmailVerificationCode([FromBody]EmailVerificationCodeModel model)
        {
            if (string.IsNullOrEmpty(model.Code))
            {
                throw new InvalidOperationException();
            }

            var request = await this.userOwnedDbContext.Set<EmailVerificationRequest>().SingleOrDefaultAsync();

            if (request != null)
            {
                if (request.NumberOfVerificationAttempts >= this.emailVerificationSettings.MaxNumberOfAttempts)
                {
                    return new ConfirmEmailVerificationCodeResultModel
                    {
                        HasErrors = true,
                        ErrorType = EmailVerificationErrorType.MaximumAttemptsReached
                    };
                }

                if (request.Code == model.Code)
                {
                    this.loggedInUserProvider.User.HasVerifiedEmail = true;

                    this.headerClientNotifier.Notify(this.Response, ApiSystemEvent.LoggedInUserChanged);

                    await this.userOwnedDbContext.Set<EmailVerificationRequest>().RemoveAllAsync();

                    await this.rootDbContext.CommitTogetherAsync(this.userOwnedDbContext);

                    return new ConfirmEmailVerificationCodeResultModel
                    {
                        HasErrors = false
                    };
                }
                else
                {
                    request.NumberOfVerificationAttempts++;

                    await this.userOwnedDbContext.SaveChangesAsync();

                    return new ConfirmEmailVerificationCodeResultModel
                    {
                        HasErrors = true,
                        ErrorType = request.NumberOfVerificationAttempts < this.emailVerificationSettings.MaxNumberOfAttempts
                            ? EmailVerificationErrorType.InvalidCode
                            : EmailVerificationErrorType.MaximumAttemptsReached
                    };
                }
            }
            else
            {
                throw new Exception();
            }
        }

        [RequireUserLoggedIn]
        [HttpPost(nameof(ResendEmailVerificationCode))]
        public async Task<ActionResult<SendEmailVerificationCodeResultModel>> ResendEmailVerificationCode()
        {
            var request = await this.userOwnedDbContext.Set<EmailVerificationRequest>().SingleAsync();

            if (request != null)
            {
                var dateWhenResendIsAvailable = this.GetResendDate(request.CreatedOn);

                if (dateWhenResendIsAvailable <= DateTime.UtcNow)
                {
                    this.userOwnedDbContext.Set<EmailVerificationRequest>().Remove(request);

                    request = await this.CreateEmailVerificationRequestAsync();

                    await this.userOwnedDbContext.SaveChangesAsync();

                    return new SendEmailVerificationCodeResultModel
                    {
                        DateWhenResendIsAvailable = this.GetResendDate(request.CreatedOn)
                    };
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                request = await this.CreateEmailVerificationRequestAsync();

                await this.userOwnedDbContext.SaveChangesAsync();

                return new SendEmailVerificationCodeResultModel
                {
                    DateWhenResendIsAvailable = this.GetResendDate(request.CreatedOn)
                };
            }
        }

        [RequireUserLoggedIn]
        [HttpPost(nameof(SendEmailVerificationCode))]
        public async Task<ActionResult<SendEmailVerificationCodeResultModel>> SendEmailVerificationCode()
        {
            var request = await this.userOwnedDbContext.Set<EmailVerificationRequest>().SingleOrDefaultAsync();

            if (request != null)
            {
                return new SendEmailVerificationCodeResultModel
                {
                    MaximumAttemptsReached = request.NumberOfVerificationAttempts >= this.emailVerificationSettings.MaxNumberOfAttempts,
                    DateWhenResendIsAvailable = this.GetResendDate(request.CreatedOn)
                };
            }
            else
            {
                request = await this.CreateEmailVerificationRequestAsync();
              
                await this.userOwnedDbContext.SaveChangesAsync();

                return new SendEmailVerificationCodeResultModel
                {
                    MaximumAttemptsReached = false,
                    DateWhenResendIsAvailable = this.GetResendDate(request.CreatedOn)
                };
            }
        }

        private async Task<EmailVerificationRequest> CreateEmailVerificationRequestAsync()
        {
            var code = this.GenerateVerificationCode();

            var emailConfirmationTemplate = new EmailConfirmationTemplate(code, this.loggedInUserProvider.User.AccountInfo.Name, this.emailSettings.LogoUrl);

            var sentEmails = await this.emailSender.SendWithCommit
            (
                x => x
                    .CreateEmail(emailConfirmationTemplate.Subject, emailConfirmationTemplate.PlainTextBody, emailConfirmationTemplate.HtmlBody)
                    .NoAttachments()
                    .FromSystemAdmin()
                    .AddRecipient(this.loggedInUserProvider.User.Email, this.loggedInUserProvider.User.AccountInfo.Name)
                    .Build()
            );

            var emailVerificationRequest = new EmailVerificationRequest
            {
                Code = code,
                SentEmailId = sentEmails.Single().Id,
                CreatedOn = DateTime.UtcNow,
                NumberOfVerificationAttempts = 0,
                UserId = this.loggedInUserProvider.User.Id
            };

            this.userOwnedDbContext.Set<EmailVerificationRequest>().Add(emailVerificationRequest);

            return emailVerificationRequest;
        }

        private string GenerateVerificationCode()
        {
            return string.Concat(Guid.NewGuid().ToString().Take(this.emailVerificationSettings.VerificationCodeLength));
        }

        private DateTime GetResendDate(DateTime createdOn) 
        {
            return createdOn.AddMinutes(this.emailVerificationSettings.ResendEmailTimeOutInMinutes);
        }
    }
}
