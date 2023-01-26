using System;
using System.Linq;
using System.Threading.Tasks;
using Augur.Data.Extensions;
using Augur.Security.Interfaces.Permissions.Services;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Identity.Web.Models;
using Ngpt.Platform.Identity.Web.Settings;
using Ngpt.Repositories.Users;
using AuthToken = Ngpt.Platform.Data.Entities.AuthTokens.AuthToken;

namespace Ngpt.Platform.Identity.Web.Services
{
    public class AuthenticationService
    {
        private readonly RootDbContext rootDbContext;
        private readonly UsersRepository usersRepo;
        private readonly ILoggedInUserProvider loggedInUserProvider;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly AuthTokenService authTokenService;

        public AuthenticationService(RootDbContext rootDbContext, UsersRepository usersRepo, ILoggedInUserProvider loggedInUserProvider, AuthenticationSettings authenticationSettings,
            AuthTokenService authTokenService)
        {
            this.rootDbContext = rootDbContext;
            this.usersRepo = usersRepo;
            this.loggedInUserProvider = loggedInUserProvider;
            this.authenticationSettings = authenticationSettings;
            this.authTokenService = authTokenService;
        }

        public async Task<SignInResponseModel> SignInUserWithCredentials(string email, string password, bool shouldCommit = true)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            if (this.usersRepo.AreCredentialsValid(email, password, out var user))
            {
                return await this.SignInUser(user, shouldCommit);
            }
            else
            {
                return null;
            }
        }

        private async Task ClearApplicableAuthTokens(int userId)
        {
            if (this.authenticationSettings.EnableMultipleDeviceLogin)
            {
                this.authTokenService.ClearCurrentAuthToken();
            }
            else
            {
                await this.authTokenService.ClearAuthTokensForUser(userId);
            }
        }

        public async Task<SignInResponseModel> SignInUser(User user, bool shouldCommit = true)
        {
            if (user.Id != 0)
            {
                await ClearApplicableAuthTokens(user.Id);
            }

            var token = new AuthToken
            {
                Guid = Guid.NewGuid().ToString(),
                User = user,
                ExpirationDate = this.authTokenService.CalculateTokenExpirationDate()
            };

            if (user.TenantId == 0)
            {
                token.Tenant = user.Tenant;
            }
            else
            {
                token.TenantId = user.TenantId;
            }

            this.rootDbContext.Set<AuthToken>().Add(token);

            if (shouldCommit)
            {
                await this.rootDbContext.SaveChangesAsync();
            }

            this.loggedInUserProvider.AssignUser(user);

            return new SignInResponseModel
            {
                Token = token.Guid,
                User = this.BuildLoggedInUserInfoModel(user)
            };
        }

        public async Task SignOutUser(int userId)
        {
            await this.ClearApplicableAuthTokens(userId);

            await this.rootDbContext.SaveChangesAsync();
        }

        public LoggedInUserInfoModel BuildLoggedInUserInfoModel(User user)
        {
            return new LoggedInUserInfoModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.AccountInfo.Name,
                IsActive = user.IsActive,
                IsSuperAdmin = user.IsSuperAdmin,
                HasVerifiedEmail = user.HasVerifiedEmail,
                IsOrgAdmin = user.IsOrgAdmin
            };
        }
    }
}
