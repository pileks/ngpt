using Augur.Web;
using Augur.Web.Controllers;
using Augur.Web.Controllers.GridModels;
using Augur.Web.Helpers;
using Google.Apis.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Entities;
using Ngpt.Data.Entities.Questions;
using Ngpt.Data.Enums;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Web.Controllers.Instructions.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ngpt.Web.Controllers.Instructions
{
    public class InstructionsController : AugurEntityWithGridController<Instruction, InstructionGridModel>
    {
        public InstructionsController(RootDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Instruction> Query()
        {
            return base.Query().Include(x => x.Language);
        }

        [RequireSuperAdminLoggedIn]
        public override async Task<IActionResult> Create([FromBody] Instruction entity)
        {
            return await base.Create(entity);
        }

        [RequireSuperAdminLoggedIn]
        public override async Task<IActionResult> Update(int id, [FromBody] Instruction entity)
        {
            return await base.Update(id, entity);
        }

        protected override async Task Update(Instruction entity, Instruction dbEntity)
        {
            dbEntity.Text = entity.Text;
            dbEntity.Language = entity.Language;

            await base.Update(entity, dbEntity);
        }

        [RequireSuperAdminLoggedIn]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpPost(nameof(ListForQuestionTypeAndLanguage))]
        [ExportToFrontendWithCustomHeaders]
        public virtual async Task<ActionResult<IEnumerable<object>>> ListForQuestionTypeAndLanguage(int languageId, UseOfLanguageQuestionType questionType, PagingQueryParameters pagingParameters, [FromBody] IColumnFilter columnFilter)
        {
            var query = this.Query()
                .Where(x => x.LanguageId == languageId)
                .Where(x => x.QuestionType == questionType);

            return await GetList(query, pagingParameters, columnFilter, this.MapListResult());
        }

        protected override IQueryable<Instruction> ApplySearchQueryFilter(IQueryable<Instruction> query, string searchQuery)
        {
            return base.ApplySearchQueryFilter(query, searchQuery).Where(x => string.IsNullOrWhiteSpace(searchQuery) || x.Text.Contains(searchQuery));
        }

        protected override GridDefinition<Instruction, InstructionGridModel> GetGridDefinition()
        {
            return new GridDefinition<Instruction, InstructionGridModel>
            {
                Columns = new List<GridColumnDefinition<InstructionGridModel>>
                {
                    new GridColumnDefinition<InstructionGridModel>
                    {
                        Title = "Text",
                        Property = x => x.Text
                    },
                    new GridColumnDefinition<InstructionGridModel>
                    {
                        Title = "Question type",
                        Property = x => x.QuestionType
                    },
                    new GridColumnDefinition<InstructionGridModel>
                    {
                        Title = "Language",
                        Property = x => x.Language
                    }
                },
                Filters = new List<GridFilterDefinition<Instruction>>
                {
                    new GridFilterDefinition<Instruction>
                    {
                        Title = "Text",
                        Property = x => x.Text
                    },
                    new GridFilterDefinition<Instruction>
                    {
                        Title = "Question type",
                        Property = x => x.QuestionType
                    },
                    new GridFilterDefinition<Instruction>
                    {
                        Title = "Language",
                        Property = x => x.Language
                    },
                },
                EntityModelMapperFn = x => new InstructionGridModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    QuestionType = x.QuestionType,
                    Language = x.Language.Name
                }
            };
        }
    }
}
