using System.Linq;
using Augur.Entity.Interfaces.Base;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Services;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;

namespace Ngpt.Web.GenericControllers
{
    public class ScopedEntitySecurityWithListController<TEntity> : ScopedEntitySecurityController<TEntity, ScopeFilter>
        where TEntity : class, IAugurEntityWithId, ITenantEntity, IAugurUserOwnedEntityWithUserId
    {
        private readonly ILoggedInTenantIdProvider loggedInTenantIdProvider;
        private readonly ILoggedInUserIdProvider loggedInUserIdProvider;

        public ScopedEntitySecurityWithListController(
            TenantUserOwnedDbContext tenantUserOwnedDbContext,
            ScopedEntitySecurityService scopedEntitySecurityService,
            ILoggedInTenantIdProvider loggedInTenantIdProvider,
            ILoggedInUserIdProvider loggedInUserIdProvider
        )
            : base(tenantUserOwnedDbContext, scopedEntitySecurityService)
        {
            this.loggedInTenantIdProvider = loggedInTenantIdProvider;
            this.loggedInUserIdProvider = loggedInUserIdProvider;
        }

        protected override IQueryable<TEntity> ApplyColumnFilter(IQueryable<TEntity> query, ScopeFilter filter)
        {
            return query
                .Where(x => !filter.ShouldFilterForOrganization || x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .Where(x => !filter.ShouldFilterOwn || x.UserId == this.loggedInUserIdProvider.UserId);
        }
    }
}