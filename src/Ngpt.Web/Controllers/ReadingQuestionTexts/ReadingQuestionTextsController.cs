using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Augur.Web.Controllers.GridModels;
using Augur.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Entities.Questions.Reading;
using Ngpt.Data.Services;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Web.Controllers.ReadingQuestionTexts.Models;
using Ngpt.Web.GenericControllers;

namespace Ngpt.Web.Controllers.ReadingQuestionTexts
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class ReadingQuestionTextsController : ScopedEntitySecurityWithGridController<ReadingQuestionText, ReadingQuestionTextGridModel>
    {
        private readonly ILoggedInUserIdProvider loggedInUserIdProvider;

        public ReadingQuestionTextsController(
            TenantUserOwnedDbContext tenantUserOwnedDbContext,
            ScopedEntitySecurityService scopedEntitySecurityService,
            ILoggedInUserIdProvider loggedInUserIdProvider
        )
            : base(tenantUserOwnedDbContext, scopedEntitySecurityService)
        {
            this.loggedInUserIdProvider = loggedInUserIdProvider;
        }

        [HttpPost(nameof(ToggleApproval))]
        public async Task<ActionResult<bool>> ToggleApproval([FromBody] ReadingQuestionText question)
        {
            var dbQuestion = await this.GetSingleOrDefaultAsync(question.Id);

            if (dbQuestion == null)
            {
                return this.NotFound();
            }

            if (dbQuestion.Approved)
            {
                dbQuestion.Approved = false;
                dbQuestion.ApproverId = null;
            }
            else
            {
                dbQuestion.Approved = true;
                dbQuestion.ApproverId = this.loggedInUserIdProvider.UserId;

                await this.Update(question, dbQuestion);
            }

            await this.TryCommit(new UpdateFailedException(typeof(ReadingQuestionText), dbQuestion.Id.ToString()));

            return this.Ok(dbQuestion.Approved);
        }

        protected override IQueryable<ReadingQuestionText> Query()
        {
            return base.Query().Include(x => x.Questions);
        }

        protected override Task<ReadingQuestionText> GetSingleOrDefaultAsync(int id)
        {
            return Query()
                .Include(x => x.Questions)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        protected override async Task Update(ReadingQuestionText entity, ReadingQuestionText dbEntity)
        {
            dbEntity.Text = entity.Text;
            dbEntity.Title = entity.Title;
            dbEntity.LanguageId = entity.LanguageId;
            dbEntity.LevelId = entity.LevelId;

            await Task.CompletedTask;
        }

        protected override IQueryable<ReadingQuestionText> ApplySearchQueryFilter(IQueryable<ReadingQuestionText> query,
            string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return query;
            }

            return query.Where(x => x.Title.Contains(searchQuery));
        }

        protected override IQueryable<ReadingQuestionText> ApplyOrdering(IQueryable<ReadingQuestionText> query)
        {
            return query.OrderBy(x => x.Id);
        }

        protected override Expression<Func<ReadingQuestionText, object>> MapListResult()
        {
            return e => new
            {
                e.Id,
                e.Title,
                e.Text,
                Language = new
                {
                    Id = e.Language.Id,
                    Name = e.Language.Name
                },
                User = new
                {
                    Id = e.User.Id,
                    AccountInfo = new
                    {
                        Id = e.User.AccountInfo.Id,
                        Name = e.User.AccountInfo.Name
                    }
                },
                Tenant = new
                {
                    Id = e.Tenant.Id,
                    DisplayName = e.Tenant.DisplayName
                },
                Approver = new
                {
                    Id = e.ApproverId,
                    AccountInfo = new
                    {
                        Id = e.Approver.AccountInfo.Id,
                        Name = e.Approver.AccountInfo.Name
                    }
                },
            };
        }

        public override Task<IActionResult> Delete(int id)
        {
            var relatedQuestions = this.DbContext().Set<ReadingQuestion>()
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Image)
                .Where(x => x.TextId == id);

            foreach (var question in relatedQuestions)
            {
                foreach (var answer in question.Answers)
                {
                    DbContext().Set<ReadingQuestionAnswer>().Remove(answer);
                }

                DbContext().Set<ReadingQuestion>().Remove(question);
            }

            return base.Delete(id);
        }

        protected override GridDefinition<ReadingQuestionText, ReadingQuestionTextGridModel> GetGridDefinition()
        {
            return new GridDefinition<ReadingQuestionText, ReadingQuestionTextGridModel>
            {
                Columns = new List<GridColumnDefinition<ReadingQuestionTextGridModel>>
                {
                    new GridColumnDefinition<ReadingQuestionTextGridModel>
                    {
                        Title = "Title",
                        Property = x => x.Title
                    },
                    new GridColumnDefinition<ReadingQuestionTextGridModel>
                    {
                        Title = "Language",
                        Property = x => x.Language
                    },
                    new GridColumnDefinition<ReadingQuestionTextGridModel>
                    {
                        Title = "Level",
                        Property = x => x.Level
                    },
                    new GridColumnDefinition<ReadingQuestionTextGridModel>
                    {
                        Title = "Questions",
                        Property = x => x.NumberOfQuestions
                    },
                    new GridColumnDefinition<ReadingQuestionTextGridModel>
                    {
                        Title = "Author",
                        Property = x => x.Author
                    },
                    new GridColumnDefinition<ReadingQuestionTextGridModel>
                    {
                        Title = "Organization",
                        Property = x => x.Organization
                    },
                    new GridColumnDefinition<ReadingQuestionTextGridModel>
                    {
                        Title = "Approved",
                        Property = x => x.Approved
                    },
                    new GridColumnDefinition<ReadingQuestionTextGridModel>
                    {
                        Title = "Approved by",
                        Property = x => x.ApprovedBy
                    },
                },
                Filters = new List<GridFilterDefinition<ReadingQuestionText>>
                {
                    new GridFilterDefinition<ReadingQuestionText>
                    {
                        Title = "Title",
                        Property = x => x.Title
                    },
                    new GridFilterDefinition<ReadingQuestionText>
                    {
                        Title = "Language",
                        Property = x => x.Language
                    },
                    new GridFilterDefinition<ReadingQuestionText>
                    {
                        Title = "Level",
                        Property = x => x.Level
                    },
                    new GridFilterDefinition<ReadingQuestionText>
                    {
                        Title = "Number of questions",
                        Property = x => x.Questions.Count
                    },
                    new GridFilterDefinition<ReadingQuestionText>
                    {
                        Title = "Author",
                        Property = x => x.User
                    },
                    new GridFilterDefinition<ReadingQuestionText>
                    {
                        Title = "Organization",
                        Property = x => x.Tenant
                    },
                    new GridFilterDefinition<ReadingQuestionText>
                    {
                        Title = "Approved",
                        Property = x => x.Approved
                    },
                    new GridFilterDefinition<ReadingQuestionText>
                    {
                        Title = "Approved by",
                        Property = x => x.Approver
                    },
                },
                EntityModelMapperFn = x => new ReadingQuestionTextGridModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Language = x.Language.Name,
                    Level = x.Level.Title,
                    Author = x.User.AccountInfo.Name,
                    Organization = x.Tenant.DisplayName,
                    NumberOfQuestions = x.Questions.Count,
                    Approved = x.Approved,
                    ApprovedBy = x.Approver.AccountInfo.Name
                }
            };
        }
    }
}