using System.Linq;
using System.Threading.Tasks;
using Augur.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Entities;
using Ngpt.Platform.Identity.Web.Authorization;

namespace Ngpt.Web.Controllers.Levels
{
    [RequireSuperAdminLoggedIn("Get", "List")]
    public class LevelsController : AugurEntityController<Level>
    {
        public LevelsController(
            RootDbContext rootDbContext)
            : base(rootDbContext)
        {

        }

        protected override IQueryable<Level> ApplySearchQueryFilter(IQueryable<Level> query, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return query;
            }

            return query.Where(x => x.Title.Contains(searchQuery));
        }

        public override Task<IActionResult> Create([FromBody] Level entity)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IActionResult> Update(int id, [FromBody] Level entity)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}