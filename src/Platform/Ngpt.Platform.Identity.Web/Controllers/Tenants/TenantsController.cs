using Augur.Web;
using Augur.Web.Controllers;
using Augur.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Web.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ngpt.Platform.Identity.Web.Controllers.Tenants
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class TenantsController : AugurEntityController<Tenant>
    {
        private readonly ILoggedInUserProvider loggedInUserProvider;

        public TenantsController(
            RootDbContext dbContext,
            ILoggedInUserProvider loggedInUserProvider
            ) : base(dbContext)
        {
            this.loggedInUserProvider = loggedInUserProvider;
        }

        [RequireSuperAdminLoggedIn]
        public override async Task<IActionResult> Create([FromBody] Tenant entity)
        {
            return await base.Create(entity);
        }

        [RequireSuperAdminLoggedIn]
        public override async Task<IActionResult> Update(int id, [FromBody] Tenant entity)
        {
            return await base.Update(id, entity);
        }

        [RequireSuperAdminLoggedIn]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [RequireSuperAdminLoggedIn]
        public override async Task<ActionResult<IEnumerable<object>>> List(PagingQueryParameters pagingParameters, [FromBody] IColumnFilter columnFilter)
        {
            return await base.List(pagingParameters, columnFilter);
        }

        [HttpPost(nameof(ListForGridFilter))]
        [ExportToFrontendWithCustomHeaders]
        public async Task<ActionResult<IEnumerable<object>>> ListForGridFilter(PagingQueryParameters pagingParameters, [FromBody] IColumnFilter columnFilter)
        {
            IQueryable<Tenant> query;

            if (this.loggedInUserProvider.User.IsSuperAdmin)
            {
                query = this.DbContext().Set<Tenant>();
            }
            else
            {
                query = this.DbContext().Set<Tenant>().Where(x => x.Id == this.loggedInUserProvider.User.TenantId);
            }

            return await GetList(query, pagingParameters, columnFilter, this.MapListResult());
        }
    }
}
