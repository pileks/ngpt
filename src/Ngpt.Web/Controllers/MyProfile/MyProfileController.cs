using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Augur.Web.Controllers;
using Ngpt.Data.DbContexts;
using Ngpt.Web.Controllers.MyProfile.Models;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Data.Entities.AccountInfos;
using Ngpt.Platform.Identity.Web.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Ngpt.Web.Controllers.MyProfile
{
    [RequireUserLoggedIn]
    public class MyProfileController : AugurApiController
    {
        private readonly ILoggedInUserProvider loggedInUserProvider;
        private readonly UserOwnedDbContext userOwnedDbContext;

        public MyProfileController(ILoggedInUserProvider loggedInUserProvider, UserOwnedDbContext userOwnedDbContext) : base()
        {
            this.loggedInUserProvider = loggedInUserProvider;
            this.userOwnedDbContext = userOwnedDbContext;
        }

        [RequireUserLoggedIn]
        [HttpGet(nameof(GetMyProfileInfo))]
        public async Task<ActionResult<MyProfileInfoModel>> GetMyProfileInfo()
        {
            var accountInfo = await this.userOwnedDbContext.Set<AccountInfo>().SingleAsync();

            return this.Ok(new MyProfileInfoModel
            {
                AccountInfo = new AccountInfoModel
                {
                    Name = accountInfo.Name,
                    Email = this.loggedInUserProvider.User.Email
                }
            });
        }
    }
}
