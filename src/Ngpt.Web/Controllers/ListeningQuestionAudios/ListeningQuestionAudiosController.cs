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
using Ngpt.Data.Entities.Questions.Listening;
using Ngpt.Data.Services;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;
using Ngpt.Web.Controllers.ListeningQuestionAudios.Models;
using Ngpt.Web.GenericControllers;

namespace Ngpt.Web.Controllers.ListeningQuestionAudios
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class ListeningQuestionAudiosController : ScopedEntitySecurityWithGridController<ListeningQuestionAudio, ListeningQuestionAudioGridModel>
    {
        private readonly ILoggedInUserIdProvider loggedInUserIdProvider;

        public ListeningQuestionAudiosController(
            TenantUserOwnedDbContext tenantUserOwnedDbContext,
            ScopedEntitySecurityService scopedEntitySecurityService,
            ILoggedInTenantIdProvider loggedInTenantIdProvider,
            ILoggedInUserIdProvider loggedInUserIdProvider
        )
            : base(tenantUserOwnedDbContext, scopedEntitySecurityService)
        {
            this.loggedInUserIdProvider = loggedInUserIdProvider;
        }

        [HttpPost(nameof(ToggleApproval))]
        public async Task<ActionResult<bool>> ToggleApproval([FromBody] ListeningQuestionAudio question)
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

            await this.TryCommit(new UpdateFailedException(typeof(ListeningQuestionAudio), dbQuestion.Id.ToString()));

            return this.Ok(dbQuestion.Approved);
        }

        protected override IQueryable<ListeningQuestionAudio> Query()
        {
            return base.Query().Include(x => x.Questions);
        }

        protected override Task<ListeningQuestionAudio> GetSingleOrDefaultAsync(int id)
        {
            return Query()
                .Include(x => x.Questions)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        protected override async Task Update(ListeningQuestionAudio entity, ListeningQuestionAudio dbEntity)
        {
            dbEntity.ResourceId = entity.ResourceId;
            dbEntity.Title = entity.Title;
            dbEntity.LanguageId = entity.LanguageId;
            dbEntity.LevelId = entity.LevelId;

            await Task.CompletedTask;
        }

        protected override IQueryable<ListeningQuestionAudio> ApplySearchQueryFilter(IQueryable<ListeningQuestionAudio> query, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return query;
            }

            return query.Where(x => x.Title.Contains(searchQuery));
        }

        protected override IQueryable<ListeningQuestionAudio> ApplyOrdering(IQueryable<ListeningQuestionAudio> query)
        {
            return query.OrderBy(x => x.Id);
        }

        protected override Expression<Func<ListeningQuestionAudio, object>> MapListResult()
        {
            return e => new
            {
                e.Id,
                e.Title,
                e.Resource,
                e.ResourceId,
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
                }
            };
        }

        public override Task<IActionResult> Delete(int id)
        {
            var relatedQuestions = this.DbContext().Set<ListeningQuestion>()
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Image)
                .Where(x => x.AudioId == id);

            foreach (var question in relatedQuestions)
            {
                foreach (var answer in question.Answers)
                {
                    this.DbContext().Set<ListeningQuestionAnswer>().Remove(answer);
                }

                this.DbContext().Set<ListeningQuestion>().Remove(question);
            }

            return base.Delete(id);
        }

        protected override GridDefinition<ListeningQuestionAudio, ListeningQuestionAudioGridModel> GetGridDefinition()
        {
            return new GridDefinition<ListeningQuestionAudio, ListeningQuestionAudioGridModel>
            {
                Columns = new List<GridColumnDefinition<ListeningQuestionAudioGridModel>>
                {
                    new GridColumnDefinition<ListeningQuestionAudioGridModel>
                    {
                        Title = "Title",
                        Property = x => x.Title
                    },
                    new GridColumnDefinition<ListeningQuestionAudioGridModel>
                    {
                        Title = "Language",
                        Property = x => x.Language
                    },
                    new GridColumnDefinition<ListeningQuestionAudioGridModel>
                    {
                        Title = "Level",
                        Property = x => x.Level
                    },
                    new GridColumnDefinition<ListeningQuestionAudioGridModel>
                    {
                        Title = "Number of questions",
                        Property = x => x.NumberOfQuestions
                    },
                    new GridColumnDefinition<ListeningQuestionAudioGridModel>
                    {
                        Title = "Author",
                        Property = x => x.Author
                    },
                    new GridColumnDefinition<ListeningQuestionAudioGridModel>
                    {
                        Title = "Organization",
                        Property = x => x.Organization
                    },
                    new GridColumnDefinition<ListeningQuestionAudioGridModel>
                    {
                        Title = "Approved",
                        Property = x => x.Approved
                    },
                    new GridColumnDefinition<ListeningQuestionAudioGridModel>
                    {
                        Title = "Approved by",
                        Property = x => x.ApprovedBy
                    },
                    new GridColumnDefinition<ListeningQuestionAudioGridModel>
                    {
                        Title = "File added",
                        Property = x => x.FileAdded
                    },
                },
                Filters = new List<GridFilterDefinition<ListeningQuestionAudio>>
                {
                    new GridFilterDefinition<ListeningQuestionAudio>
                    {
                        Title = "Title",
                        Property = x => x.Title
                    },
                    new GridFilterDefinition<ListeningQuestionAudio>
                    {
                        Title = "Language",
                        Property = x => x.Language
                    },
                    new GridFilterDefinition<ListeningQuestionAudio>
                    {
                        Title = "Level",
                        Property = x => x.Level
                    },
                    new GridFilterDefinition<ListeningQuestionAudio>
                    {
                        Title = "Number of questions",
                        Property = x => x.Questions.Count
                    },
                    new GridFilterDefinition<ListeningQuestionAudio>
                    {
                        Title = "Author",
                        Property = x => x.User
                    },
                    new GridFilterDefinition<ListeningQuestionAudio>
                    {
                        Title = "Organization",
                        Property = x => x.Tenant
                    },
                    new GridFilterDefinition<ListeningQuestionAudio>
                    {
                        Title = "Approved",
                        Property = x => x.Approved
                    },
                    new GridFilterDefinition<ListeningQuestionAudio>
                    {
                        Title = "Approved by",
                        Property = x => x.Approver
                    },
                    new GridFilterDefinition<ListeningQuestionAudio>
                    {
                        Title = "File added",
                        Property = x => x.ResourceId.HasValue
                    },
                },
                EntityModelMapperFn = x => new ListeningQuestionAudioGridModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Language = x.Language.Name,
                    Level = x.Level.Title,
                    Author = x.User.AccountInfo.Name,
                    Organization = x.Tenant.DisplayName,
                    NumberOfQuestions = x.Questions.Count,
                    Approved = x.Approved,
                    ApprovedBy = x.Approver.AccountInfo.Name,
                    FileAdded = x.ResourceId.HasValue
                }
            };
        }
    }
}