using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;

namespace Ngpt.Data.Services
{
    public class ScopedEntitySecurityService
    {
        private readonly TenantUserOwnedDbContext tenantUserOwnedDbContext;
        private readonly TenantOwnedDbContext tenantOwnedDbContext;
        private readonly OrgAdminDbContext orgAdminOwnedDbContext;
        private readonly SuperAdminDbContext superAdminOwnedDbContext;
        private readonly RootDbContext rootDbContext;
        private readonly ILoggedInUserProvider loggedInUserProvider;

        public ScopedEntitySecurityService(
                TenantUserOwnedDbContext tenantUserOwnedDbContext,
                TenantOwnedDbContext tenantOwnedDbContext,
                OrgAdminDbContext orgAdminOwnedDbContext,
                SuperAdminDbContext superAdminOwnedDbContext,
                RootDbContext rootDbContext,
                ILoggedInUserProvider loggedInUserProvider
            )
        {
            this.tenantUserOwnedDbContext = tenantUserOwnedDbContext;
            this.tenantOwnedDbContext = tenantOwnedDbContext;
            this.orgAdminOwnedDbContext = orgAdminOwnedDbContext;
            this.superAdminOwnedDbContext = superAdminOwnedDbContext;
            this.rootDbContext = rootDbContext;
            this.loggedInUserProvider = loggedInUserProvider;
        }

        public DbContext DbContext()
        {
            if (this.loggedInUserProvider.User.IsSuperAdmin)
            {
                return this.superAdminOwnedDbContext;
            }
            else if (this.loggedInUserProvider.User.IsOrgAdmin)
            {
                return this.orgAdminOwnedDbContext;
            }
            else
            {
                return this.tenantUserOwnedDbContext;
            }
        }
    }
}
