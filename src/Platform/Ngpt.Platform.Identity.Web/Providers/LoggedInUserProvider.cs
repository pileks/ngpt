using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.AuthTokens;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Identity.Web.Services;

namespace Ngpt.Platform.Identity.Web.Providers
{
    public class LoggedInUserProvider : ILoggedInUserProvider
    {
        private readonly AuthTokenService tokenService;
        private readonly RootDbContext rootDbContext;

        public LoggedInUserProvider(AuthTokenService tokenService, RootDbContext rootDbContext)
        {
            this.tokenService = tokenService;
            this.rootDbContext = rootDbContext;
        }
        private User user;
        public User User
        {
            get
            {
                if (this.user == null)
                {
                    this.user = this.LoadLoggedInUser();
                }

                return this.user;
            }
        }

        public bool IsUserLoggedIn =>
            this.User != null;

        public User LoadLoggedInUser()
        {
            var authToken = this.tokenService.GetAuthToken();
            return authToken?.User;
        }

        public void AssignUser(User user)
        {
            this.user = user;
        }
    }
}
