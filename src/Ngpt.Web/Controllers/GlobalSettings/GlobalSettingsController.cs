using System.Threading.Tasks;
using Augur.Web.Controllers;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Identity.Web.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ngpt.Web.Controllers.GlobalSettings
{
    [RequireSuperAdminLoggedIn]
    public class GlobalSettingsController : AugurApiController
    {
        private readonly RootDbContext rootDbContext;

        public GlobalSettingsController(RootDbContext rootDbContext)
        {
            this.rootDbContext = rootDbContext;
        }

        [HttpGet(nameof(Get))]
        public virtual async Task<ActionResult<Platform.Data.Entities.GlobalSettings>> Get()
        {
            var entity = await this.rootDbContext.Set<Platform.Data.Entities.GlobalSettings>().SingleOrDefaultAsync();

            return this.Ok(entity);
        }

        [HttpPost(nameof(Update))]
        public virtual async Task<ActionResult<Platform.Data.Entities.GlobalSettings>> Update([FromBody]Platform.Data.Entities.GlobalSettings model)
        {
            var entity = await this.rootDbContext.Set<Platform.Data.Entities.GlobalSettings>().SingleOrDefaultAsync();

            await this.rootDbContext.SaveChangesAsync();

            return this.Ok(entity);
        }
    }
}
