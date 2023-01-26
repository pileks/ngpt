using Microsoft.AspNetCore.Mvc;
using Augur.Web.Controllers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Web.Controllers.Dashboard.Models;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;

namespace Ngpt.Web.Controllers.Dashboard
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class DashboardController : AugurApiController
    {
        private readonly TenantOwnedDbContext tenantOwnedDbContext;

        public DashboardController(ILoggedInUserProvider loggedInUserProvider, TenantOwnedDbContext tenantOwnedDbContext)
        {
            this.tenantOwnedDbContext = tenantOwnedDbContext;
        }

        [HttpGet(nameof(GetDashboardInfo))]
        public ActionResult<DashboardInfoModel> GetDashboardInfo()
        {
            return this.Ok(new DashboardInfoModel
            {
            });
        }
    }
}
