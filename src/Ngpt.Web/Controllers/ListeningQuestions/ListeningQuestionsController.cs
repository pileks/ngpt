using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Augur.Web.Controllers.GridModels;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Entities.Questions.Listening;
using Ngpt.Data.Services;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;
using Ngpt.Web.Controllers.ListeningQuestions.Models;
using Ngpt.Web.GenericControllers;

namespace Ngpt.Web.Controllers.ListeningQuestions
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class ListeningQuestionsController : ScopedEntitySecurityWithGridController<ListeningQuestion, ListeningQuestionGridModel>
    {
        private readonly TenantUserOwnedDbContext tenantUserOwnedDbContext;

        public ListeningQuestionsController(
                TenantUserOwnedDbContext tenantUserOwnedDbContext,
                ScopedEntitySecurityService scopedEntitySecurityService,
                ILoggedInTenantIdProvider loggedInTenantIdProvider,
                ILoggedInUserIdProvider loggedInUserIdProvider
            )
            : base(tenantUserOwnedDbContext, scopedEntitySecurityService)
        {
            this.tenantUserOwnedDbContext = tenantUserOwnedDbContext;
        }

        protected override async Task<ListeningQuestion> GetSingleOrDefaultAsync(int id)
        {
            return await DbSet()
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Image)
                .Include(x => x.Audio)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override ListeningQuestion BeforeCreate(ListeningQuestion entity)
        {
            //Sanitize entity
            entity.Audio = null;

            return base.BeforeCreate(entity);
        }

        protected override async Task Update(ListeningQuestion entity, ListeningQuestion dbEntity)
        {
            dbEntity.AnswerType = entity.AnswerType;
            dbEntity.QuestionText = entity.QuestionText;
            dbEntity.AudioId = entity.AudioId;

            AddUpdateRemoveEfCollection(
                dbEntity.Answers,
                entity.Answers,
                (dbE, e) => dbE.Id != 0 && dbE.Id == e.Id,
                answer =>
                {
                    var dbAnswer = new ListeningQuestionAnswer();

                    CopyAnswerFields(dbAnswer, answer);

                    return dbAnswer;
                },
                (dbAnswer, answer) =>
                {
                    CopyAnswerFields(dbAnswer, answer);
                });

            await Task.CompletedTask;
        }

        private void CopyAnswerFields(ListeningQuestionAnswer dst, ListeningQuestionAnswer src)
        {
            dst.Text = src.Text;
            dst.ImageId = src.ImageId;
            dst.IsCorrect = src.IsCorrect;
            dst.Ordinal = src.Ordinal;
        }

        protected override IQueryable<ListeningQuestion> ApplySearchQueryFilter(IQueryable<ListeningQuestion> query, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return query;
            }

            return query.Where(x => x.QuestionText.Contains(searchQuery));
        }

        protected override Expression<Func<ListeningQuestion, object>> MapListResult()
        {
            return e => new
            {
                e.Id,
                e.QuestionText,
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

        protected override async Task Delete(ListeningQuestion dbEntity)
        {
            foreach (var answer in dbEntity.Answers)
            {
                this.DbContext().Set<ListeningQuestionAnswer>().Remove(answer);
            }

            await base.Delete(dbEntity);
        }

        protected override GridDefinition<ListeningQuestion, ListeningQuestionGridModel> GetGridDefinition()
        {
            return new GridDefinition<ListeningQuestion, ListeningQuestionGridModel>
            {
                Columns = new List<GridColumnDefinition<ListeningQuestionGridModel>>
                {
                    new GridColumnDefinition<ListeningQuestionGridModel>
                    {
                        Title = "Question text",
                        Property = x => x.QuestionText
                    },
                    new GridColumnDefinition<ListeningQuestionGridModel>
                    {
                        Title = "Author",
                        Property = x => x.Author
                    },
                    new GridColumnDefinition<ListeningQuestionGridModel>
                    {
                        Title = "Organization",
                        Property = x => x.Organization
                    },
                },
                Filters = new List<GridFilterDefinition<ListeningQuestion>>
                {
                    new GridFilterDefinition<ListeningQuestion>
                    {
                        Title = "Question text",
                        Property = x => x.QuestionText
                    },
                    new GridFilterDefinition<ListeningQuestion>
                    {
                        Title = "Author",
                        Property = x => x.User
                    },
                    new GridFilterDefinition<ListeningQuestion>
                    {
                        Title = "Organization",
                        Property = x => x.Tenant
                    },
                },
                EntityModelMapperFn = x => new ListeningQuestionGridModel
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    Author = x.User.AccountInfo.Name,
                    Organization = x.Tenant.DisplayName
                }
            };
        }
    }
}