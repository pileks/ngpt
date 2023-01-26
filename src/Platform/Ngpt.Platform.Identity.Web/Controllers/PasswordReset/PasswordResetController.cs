using System;
using System.Linq;
using System.Threading.Tasks;
using Augur.Data.Extensions;
using Augur.Web.Controllers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities;
using Ngpt.Platform.Data.Entities.ChangePasswordRequests;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Identity.Web.Controllers.PasswordReset.Models;
using Ngpt.Platform.Identity.Web.EmailTemplates.PasswordReset;
using Ngpt.Repositories.Users;
using Ngpt.Platform.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ngpt.Platform.Identity.Web.Services;
using AuthToken = Ngpt.Platform.Data.Entities.AuthTokens.AuthToken;

namespace Ngpt.Platform.Identity.Web.Controllers.PasswordReset
{
    public class PasswordResetController : AugurApiController
    {
        private readonly RootDbContext rootDbContext;
        private readonly UsersRepository usersRepo;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly EmailSender<RootDbContext> emailSender;
        private readonly IConfiguration configuration;
        private readonly AuthTokenService authTokenService;

        private readonly string logoUrl;
        public PasswordResetController(RootDbContext rootDbContext, UsersRepository usersRepo, 
            IPasswordHasher<User> passwordHasher, EmailSender<RootDbContext> emailSender, IConfiguration configuration, AuthTokenService authTokenService)
        {
            this.rootDbContext = rootDbContext;
            this.usersRepo = usersRepo;
            this.passwordHasher = passwordHasher;
            this.emailSender = emailSender;
            this.configuration = configuration;
            this.authTokenService = authTokenService;

            this.logoUrl = this.configuration.GetSection("Email")["LogoUrl"];
        }

        [HttpPost(nameof(RequestPasswordReset))]
        public async Task<IActionResult> RequestPasswordReset([FromBody]RequestPasswordResetModel model)
        {
            var applicationUrl = this.configuration.GetValue<string>("ApplicationUrl");

            var user = await this.rootDbContext.Set<User>()
                .Include(u => u.AccountInfo)
                .SingleOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                return this.Ok();
            }

            await this.rootDbContext.Set<ChangePasswordRequest>().RemoveAsync(t => t.UserId == user.Id);

            var uid = Guid.NewGuid().ToString();

            var template = new PasswordResetTemplate(applicationUrl, uid, user.AccountInfo.Name, this.logoUrl);

            var sentEmails = await this.emailSender.SendWithCommit
            (
                x => x
                    .CreateEmail(template.Subject, template.PlainTextBody, template.HtmlBody)
                    .NoAttachments()
                    .FromSystemAdmin()
                    .AddRecipient(user.Email, user.AccountInfo.Name)
                    .Build()
            );


            this.rootDbContext.Set<ChangePasswordRequest>().Add(new ChangePasswordRequest()
            {
                Uid = uid,
                SentEmailId = sentEmails.Single().Id,
                UserId = user.Id
            });

            await this.rootDbContext.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost(nameof(SetNewPassword))]
        public async Task<IActionResult> SetNewPassword([FromBody]SetNewPasswordModel model)
        {
            var changePasswordRequest = await this.rootDbContext.Set<ChangePasswordRequest>()
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Uid == model.Uid);

            if (changePasswordRequest == null) return this.Ok();
            
            var user = changePasswordRequest.User;

            user.Password = this.passwordHasher.HashPassword(user, model.Password);

            await this.authTokenService.ClearAuthTokensForUser(user.Id);
            await this.rootDbContext.Set<ChangePasswordRequest>().RemoveAsync(t => t.UserId == user.Id);

            await this.rootDbContext.SaveChangesAsync();

            return this.Ok();
        }
    }
}
