using System.Threading.Tasks;
using Augur.Data.Extensions;
using Augur.Web.Controllers;
using Augur.Web.Helpers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Identity.Web.Controllers.Authentication.Models;
using Ngpt.Platform.Identity.Web.Models;
using Ngpt.Platform.Identity.Web.Services;
using Microsoft.AspNetCore.Mvc;
using AuthToken = Ngpt.Platform.Data.Entities.AuthTokens.AuthToken;

namespace Ngpt.Platform.Identity.Web.Controllers.Authentication
{

    public class AuthenticationController : AugurApiController
    {
        private readonly UserOwnedDbContext userOwnedDbContext;
        private readonly AuthenticationService authenticationService;
        private readonly ILoggedInUserProvider loggedInUserProvider;

        public AuthenticationController(UserOwnedDbContext userOwnedDbContext, AuthenticationService authenticationService,
            ILoggedInUserProvider loggedInUserProvider)
        {
            this.userOwnedDbContext = userOwnedDbContext;
            this.authenticationService = authenticationService;
            this.loggedInUserProvider = loggedInUserProvider;
        }

        [HttpPost(nameof(SignIn))]
        public async Task<ValidationResult<SignInResponseModel>> SignIn([FromBody] SignInModel model)
        {
            var responseModel = await this.authenticationService.SignInUserWithCredentials(model.Email, model.Password);

            if (responseModel != null)
            {
                return responseModel;
            }
            else
            {
                return this.ValidationError();
            }
        }

        [RequireUserLoggedIn]
        [HttpPost(nameof(SignOut))]
        public async Task<IActionResult> SignOut()
        {
            await this.authenticationService.SignOutUser(this.loggedInUserProvider.User.Id);

            return this.Ok();
        }

        [RequireUserLoggedIn]
        [HttpGet(nameof(GetLoggedInUserInfo))]
        public ActionResult<LoggedInUserInfoModel> GetLoggedInUserInfo()
        {
            return this.authenticationService.BuildLoggedInUserInfoModel(this.loggedInUserProvider.User);
        }
    }
}
