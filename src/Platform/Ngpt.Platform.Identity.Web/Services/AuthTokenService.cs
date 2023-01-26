using System;
using System.Linq;
using System.Threading.Tasks;
using Augur.Data.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.AuthTokens;
using Ngpt.Platform.Identity.Web.Providers;
using Ngpt.Platform.Identity.Web.Settings;

namespace Ngpt.Platform.Identity.Web.Services
{
    public class AuthTokenService
    {
        private readonly AuthenticationSettings authenticationSettings;
        private readonly RootDbContext rootDbContext;

        public AuthTokenService(RootDbContext rootDbContext, AuthenticationSettings authenticationSettings)
        {
            this.rootDbContext = rootDbContext;
            this.authenticationSettings = authenticationSettings;
        }

        public string RequestToken { get; set; }
        private AuthToken _authToken;

        public AuthToken GetAuthToken()
        {
            if (this._authToken != null)
            {
                return this._authToken;
            }

            var token = this.RequestToken;

            if (string.IsNullOrWhiteSpace(token)) return null;

            var authToken = this.rootDbContext.Set<AuthToken>()
                .Include(t => t.User)
                .ThenInclude(u => u.AccountInfo)
                .OrderByDescending(t => t.Id)
                .Where(t => t.ExpirationDate > DateTime.UtcNow)
                .FirstOrDefault(t => t.Guid == token);

            this._authToken = authToken;
     
            return authToken;
        }

        public async Task ExtendAuthTokenExpirationIfExists()
        {
            var authToken = GetAuthToken();

            if (authToken != null)
            {
                authToken.ExpirationDate = this.CalculateTokenExpirationDate();
                await this.rootDbContext.SaveChangesAsync();
            }
        }

        public DateTime CalculateTokenExpirationDate()
        {
            return DateTime.UtcNow.AddHours(this.authenticationSettings.TokenExpirationHours ??
                                            AuthenticationSettings.DefaultTokenExpirationHours);
        }

        public void ClearCurrentAuthToken()
        {
            var authToken = this.GetAuthToken();
            if (authToken == null)
            {
                return;
            }

            this.rootDbContext.Set<AuthToken>().Remove(authToken);
        }

        public async Task ClearAuthTokensForUser(int userId)
        {
            await this.rootDbContext.Set<AuthToken>().RemoveAsync(t => t.UserId == userId);
        }
    }
}
