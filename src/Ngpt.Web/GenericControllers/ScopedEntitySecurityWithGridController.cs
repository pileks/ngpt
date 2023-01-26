using Augur.Entity.Interfaces.Base;
using Augur.Web.Controllers;
using Augur.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Services;

namespace Ngpt.Web.GenericControllers
{
    public abstract class ScopedEntitySecurityWithGridController<TEntity, TGridModel> : ScopedEntitySecurityWithGridController<TEntity, TGridModel, IColumnFilter>
        where TEntity : class, IAugurEntityWithId, ITenantEntity
        where TGridModel : class
    {
        private readonly ScopedEntitySecurityService scopedEntitySecurityService;

        public ScopedEntitySecurityWithGridController(
            TenantUserOwnedDbContext tenantUserOwnedDbContext,
            ScopedEntitySecurityService scopedEntitySecurityService
        )
            : base(tenantUserOwnedDbContext, scopedEntitySecurityService)
        {
            this.scopedEntitySecurityService = scopedEntitySecurityService;
        }
    }

    public abstract class ScopedEntitySecurityWithGridController<TEntity, TGridModel, TColumnFilter> : AugurEntityWithGridController<TEntity, TGridModel, TColumnFilter>
        where TEntity : class, IAugurEntityWithId, ITenantEntity
        where TColumnFilter : class, IColumnFilter
        where TGridModel : class
    {
        private readonly ScopedEntitySecurityService scopedEntitySecurityService;

        public ScopedEntitySecurityWithGridController(
            TenantUserOwnedDbContext tenantUserOwnedDbContext,
            ScopedEntitySecurityService scopedEntitySecurityService
        )
            : base(tenantUserOwnedDbContext)
        {
            this.scopedEntitySecurityService = scopedEntitySecurityService;
        }

        protected override DbContext DbContext()
        {
            return this.scopedEntitySecurityService.DbContext();
        }
    }
}