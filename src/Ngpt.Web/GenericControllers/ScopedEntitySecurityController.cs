using System.Linq;
using Augur.Entity.Interfaces.Base;
using Augur.Web.Controllers;
using Augur.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Services;

namespace Ngpt.Web.GenericControllers
{
    public class ScopedEntitySecurityController<TEntity> : ScopedEntitySecurityController<TEntity, IColumnFilter>
        where TEntity : class, IAugurEntityWithId, ITenantEntity
    {
        private readonly ScopedEntitySecurityService scopedEntitySecurityService;

        public ScopedEntitySecurityController(
            TenantUserOwnedDbContext tenantUserOwnedDbContext,
            ScopedEntitySecurityService scopedEntitySecurityService
        )
            : base(tenantUserOwnedDbContext, scopedEntitySecurityService)
        {
            this.scopedEntitySecurityService = scopedEntitySecurityService;
        }
    }

    public class ScopedEntitySecurityController<TEntity, TColumnFilter> : AugurEntityController<TEntity, int, TColumnFilter>
        where TEntity : class, IAugurEntityWithId, ITenantEntity
        where TColumnFilter : class, IColumnFilter
    {
        private readonly ScopedEntitySecurityService scopedEntitySecurityService;

        public ScopedEntitySecurityController(
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