using System;
using System.Threading.Tasks;
using Augur.Web.Controllers;
using Augur.Web.Helpers;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Identity.Web.Controllers.ChangePassword.Models;
using Ngpt.Repositories.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ngpt.Platform.Identity.Web.Controllers.ChangePassword
{
    [RequireUserLoggedIn]
    public class ChangePasswordController : AugurApiController
    {
        private readonly UsersRepository usersRepo;
        private readonly ILoggedInUserProvider loggedInUserProvider;
        private readonly IPasswordHasher<User> passwordHasher;

        public ChangePasswordController(UsersRepository usersRepo, ILoggedInUserProvider loggedInUserProvider, IPasswordHasher<User> passwordHasher)
        {
            this.usersRepo = usersRepo;
            this.loggedInUserProvider = loggedInUserProvider;
            this.passwordHasher = passwordHasher;
        }

        [HttpPost(nameof(ChangePassword))]
        public async Task<ValidationResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                throw new InvalidOperationException();
            }

            if (this.usersRepo.AreCredentialsValid(this.loggedInUserProvider.User.Email, model.OldPassword, out var user))
            {
                this.loggedInUserProvider.User.Password = this.passwordHasher.HashPassword(user, model.NewPassword);

                await this.usersRepo.SaveChangesAsync();

                return this.Ok();
            }
            else
            {
                return this.ValidationError();
            }
        }
    }
}
