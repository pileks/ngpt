using System.Linq;
using System.Threading.Tasks;
using Augur.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Entities;
using Ngpt.Platform.Identity.Web.Authorization;

namespace Ngpt.Web.Controllers.Languages
{
    [RequireSuperAdminLoggedIn("Get", "List")]
    public class LanguagesController : AugurEntityController<Language>
    {
        public LanguagesController(
            RootDbContext rootDbContext)
            : base(rootDbContext)
        {

        }

        protected override IQueryable<Language> ApplySearchQueryFilter(IQueryable<Language> query, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return query;
            }

            return query.Where(x => x.Name.Contains(searchQuery));
        }

        public override Task<IActionResult> Create([FromBody] Language entity)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IActionResult> Update(int id, [FromBody] Language entity)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IActionResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}