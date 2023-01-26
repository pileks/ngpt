using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Augur.Web.Controllers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Identity.Web.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Augur.Web.Helpers;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Microsoft.EntityFrameworkCore;
using Augur.Web;

namespace Ngpt.Platform.Identity.Web.Controllers.Users
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class UsersController : AugurEntityController<User>
    {
        private readonly ILoggedInUserProvider loggedInUserProvider;

        public UsersController(
            RootDbContext rootDbContext,
            ILoggedInUserProvider loggedInUserProvider
            ) : base(rootDbContext)
        {
            this.loggedInUserProvider = loggedInUserProvider;
        }

        [HttpPost]
        [RequireSuperAdminLoggedIn]
        public override Task<IActionResult> Create([FromBody] User entity)
        {
            throw new NotSupportedException();
        }

        [RequireSuperAdminLoggedIn]
        protected override Task Delete(User dbEntity)
        {
            throw new NotSupportedException();
        }

        protected override Task<User> AfterGet(User entity)
        {
            return Task.FromResult(new User
            {
                Email = entity.Email,
                IsActive = entity.IsActive,
                IsSuperAdmin = entity.IsSuperAdmin,
                IsOrgAdmin = entity.IsOrgAdmin
            });
        }

        [RequireSuperAdminLoggedIn]
        protected override async Task Update(User entity, User dbEntity)
        {
            dbEntity.Email = entity.Email;
            dbEntity.IsActive = entity.IsActive;
            dbEntity.IsSuperAdmin = entity.IsSuperAdmin;
            dbEntity.IsOrgAdmin = entity.IsOrgAdmin;

            await Task.CompletedTask;
        }

        [RequireSuperAdminLoggedIn]
        public override async Task<ActionResult<IEnumerable<object>>> List(PagingQueryParameters pagingParameters, [FromBody] IColumnFilter columnFilter)
        {
            return await base.List(pagingParameters, columnFilter);
        }

        protected override Expression<Func<User, object>> MapListResult()
        {
            return entity => new
            {
                entity.Id,
                entity.Email,
                entity.IsActive,
                entity.IsOrgAdmin,
                entity.AccountInfo.Name
            };
        }

        protected override IQueryable<User> Query()
        {
            return base.Query()
                .Include(x => x.AccountInfo);
        }

        protected override IQueryable<User> ApplySearchQueryFilter(IQueryable<User> query, string searchQuery)
        {
            return string.IsNullOrEmpty(searchQuery)
                ? query
                : query.Where(x => x.Email.Contains(searchQuery));
        }

        [HttpPost(nameof(ListForGridFilter))]
        [ExportToFrontendWithCustomHeaders]
        public async Task<ActionResult<IEnumerable<object>>> ListForGridFilter(PagingQueryParameters pagingParameters, [FromBody] IColumnFilter columnFilter)
        {
            IQueryable<User> query;

            if (this.loggedInUserProvider.User.IsSuperAdmin)
            {
                query = this.DbContext().Set<User>();
            }
            else if (this.loggedInUserProvider.User.IsOrgAdmin)
            {
                query = this.DbContext().Set<User>().Where(x => x.TenantId == this.loggedInUserProvider.User.TenantId);
            }
            else
            {
                query = this.DbContext().Set<User>().Where(x => x.Id == this.loggedInUserProvider.User.Id);
            }

            return await GetList(query, pagingParameters, columnFilter, this.MapListResult());
        }

    }
}
