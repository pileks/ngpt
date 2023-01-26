using System;
using System.Threading.Tasks;
using Augur.Data.Extensions;
using Augur.Data.Services;
using Augur.Security.Interfaces.Permissions.Cache;
using Augur.Web.Controllers;
using Augur.Web.Helpers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.AccessControl;
using Ngpt.Platform.Data.Entities.AccountInfos;
using Ngpt.Platform.Data.Entities.ChangePasswordRequests;
using Ngpt.Platform.Data.Entities.EmailVerificationRequests;
using Ngpt.Platform.Data.Entities.Roles;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Identity.Web.Controllers.Registration.Models;
using Ngpt.Platform.Identity.Web.Models;
using Ngpt.Platform.Identity.Web.Services;
using Ngpt.Repositories.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ngpt.Platform.Exceptions;
using AuthToken = Ngpt.Platform.Data.Entities.AuthTokens.AuthToken;

namespace Ngpt.Platform.Identity.Web.Controllers.Registration
{
    public class RegistrationController : AugurApiController
    {
        private readonly RootDbContext rootDbContext;
        private readonly UsersRepository usersRepo;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AuthenticationService authenticationService;
        private readonly ITransactionManager transactionManager;

        public RegistrationController(RootDbContext rootDbContext, UsersRepository usersRepo, IPasswordHasher<User> passwordHasher,
            AuthenticationService authenticationService, ITransactionManager transactionManager)
        {
            this.rootDbContext = rootDbContext;
            this.usersRepo = usersRepo;
            this.passwordHasher = passwordHasher;
            this.authenticationService = authenticationService;
            this.transactionManager = transactionManager;
        }

        [HttpPost(nameof(Register))]
        public async Task<ValidationResult<SignInResponseModel>> Register([FromBody] RegisterModel model)
        {
            return await this.transactionManager.UseTransactionAsync<ValidationResult<SignInResponseModel>>(async () =>
            {
                var (isSuccess, user) = await this.RegisterUserWithoutCommit(model.Email, model.Password, model.Name, model.TenantName,
                    model.HasAcceptedTermsAndPrivacyPolicy);

                await this.rootDbContext.SaveChangesAsync();

                if (isSuccess)
                {
                    user.Tenant.OrgOwnerId = user.Id;

                    var responseModel = await this.authenticationService.SignInUser(user, shouldCommit: false);

                    await this.rootDbContext.SaveChangesAsync();

                    if (responseModel != null)
                    {
                        return responseModel;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    return this.ValidationError();
                }
            });
        }

        public async Task<(bool isSuccess, User user)> RegisterUserWithoutCommit(string email, string password, string name, string tenantName, bool hasAcceptedTermsAndPrivacyPolicy)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@") || string.IsNullOrEmpty(password) || !hasAcceptedTermsAndPrivacyPolicy)
            {
                throw new SimpleVerboseException("Invalid registration details.");
            }

            var userWithSameEmail = await this.rootDbContext.Set<User>().SingleOrDefaultAsync(u => u.Email == email);

            if (userWithSameEmail == null || !userWithSameEmail.HasVerifiedEmail)
            {
                if (userWithSameEmail != null && !userWithSameEmail.HasVerifiedEmail)
                {
                    await this.rootDbContext.Set<ChangePasswordRequest>().RemoveAsync(t => t.UserId == userWithSameEmail.Id);
                    await this.rootDbContext.Set<EmailVerificationRequest>().RemoveAsync(t => t.UserId == userWithSameEmail.Id);

                    await this.rootDbContext.Set<AuthToken>().RemoveAsync(t => t.UserId == userWithSameEmail.Id);

                    await this.rootDbContext.Set<AccountInfo>().RemoveAsync(t => t.UserId == userWithSameEmail.Id);
                    this.rootDbContext.Set<User>().Remove(userWithSameEmail);
                }

                var user = new User
                {
                    Email = email,
                    IsActive = true,
                    IsOrgAdmin = true,
                    AccountInfo = new AccountInfo
                    {
                        Name = name,
                        HasAcceptedTermsAndPrivacyPolicy = true
                    }
                };

                user.Password = this.passwordHasher.HashPassword(user, password);

                this.RegisterTenantAndAssignToUser(tenantName, user);

                this.rootDbContext.Set<User>().Add(user);

                return (true, user);
            }
            else
            {
                return (false, null);
            }
        }

        private void RegisterTenantAndAssignToUser(string tenantName, User user)
        {
            var tenant = new Tenant
            {
                Name = tenantName,
                DisplayName = tenantName
            };

            this.rootDbContext.Set<Tenant>().Add(tenant);

            user.Tenant = tenant;
            user.AccountInfo.Tenant = tenant;
        }
    }
}
