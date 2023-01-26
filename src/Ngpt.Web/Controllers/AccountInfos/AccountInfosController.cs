using Augur.Web.Controllers;
using Ngpt.Web.Controllers.AccountInfos.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Augur.ApiSystemEvents;
using Ngpt.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Ngpt.Platform.Data.Entities.AccountInfos;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Services.Enums;

namespace Ngpt.Web.Controllers.AccountInfos
{
    [RequireSuperAdminLoggedIn(ignoredActions: new[] 
    {
        nameof(UpdateNameForLoggedInUser) 
    })]
    public class AccountInfosController : AugurEntityController<AccountInfo>
    {
        private readonly RootDbContext rootDbContext;
        private readonly UserOwnedDbContext userOwnedDbContext;
        private readonly ILoggedInUserProvider loggedInUserProvider;
        private readonly IHeaderClientNotifier headerClientNotifier;

        public AccountInfosController(RootDbContext rootDbContext, UserOwnedDbContext userOwnedDbContext, ILoggedInUserProvider loggedInUserProvider,
            IHeaderClientNotifier clientSystemEventNotifier) : base(rootDbContext)
        {
            this.rootDbContext = rootDbContext;
            this.userOwnedDbContext = userOwnedDbContext;
            this.rootDbContext = rootDbContext;
            this.loggedInUserProvider = loggedInUserProvider;
            this.headerClientNotifier = clientSystemEventNotifier;
        }

        [RequireUserWithVerifiedEmailLoggedIn]
        [HttpPost(nameof(UpdateNameForLoggedInUser))]
        public virtual async Task<ActionResult<UpdateNameModel>> UpdateNameForLoggedInUser([FromBody]UpdateNameModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
            {
                return this.BadRequest();
            }

            var accountInfo = await this.userOwnedDbContext.Set<AccountInfo>().SingleAsync();

            accountInfo.Name = model.Name;

            await this.userOwnedDbContext.SaveChangesAsync();

            this.headerClientNotifier.Notify(this.Response, ApiSystemEvent.LoggedInUserChanged);

            return this.Ok(new UpdateNameModel
            {
                Name = accountInfo.Name
            });
        }

        protected override AccountInfo BeforeCreate(AccountInfo entity)
        {
            throw new InvalidOperationException();
        }

        public override Task<IActionResult> Update(Int32 id, [FromBody] AccountInfo entity)
        {
            throw new InvalidOperationException();
        }

        protected override Task Delete(AccountInfo dbEntity)
        {
            throw new InvalidOperationException();
        }
    }
}