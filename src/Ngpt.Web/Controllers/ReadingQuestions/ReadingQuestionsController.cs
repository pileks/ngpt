using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Augur.Web.Controllers.GridModels;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Entities.Questions.Reading;
using Ngpt.Data.Services;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;
using Ngpt.Web.Controllers.ReadingQuestions.Models;
using Ngpt.Web.GenericControllers;

namespace Ngpt.Web.Controllers.ReadingQuestions
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class ReadingQuestionsController : ScopedEntitySecurityWithGridController<ReadingQuestion, ReadingQuestionGridModel>
    {
        private readonly TenantUserOwnedDbContext tenantUserOwnedDbContext;

        public ReadingQuestionsController(
            TenantUserOwnedDbContext tenantUserOwnedDbContext,
            ScopedEntitySecurityService scopedEntitySecurityService,
            ILoggedInTenantIdProvider loggedInTenantIdProvider,
            ILoggedInUserIdProvider loggedInUserIdProvider
        )
            : base(tenantUserOwnedDbContext, scopedEntitySecurityService)
        {
            this.tenantUserOwnedDbContext = tenantUserOwnedDbContext;
        }

        protected override async Task<ReadingQuestion> GetSingleOrDefaultAsync(int id)
        {
            return await DbSet()
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Image)
                .Include(x => x.Text)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override ReadingQuestion BeforeCreate(ReadingQuestion entity)
        {
            //Sanitize entity
            entity.Text = null;

            return base.BeforeCreate(entity);
        }

        protected override async Task Update(ReadingQuestion entity, ReadingQuestion dbEntity)
        {
            dbEntity.AnswerType = entity.AnswerType;
            dbEntity.QuestionText = entity.QuestionText;
            dbEntity.TextId = entity.TextId;

            AddUpdateRemoveEfCollection(
                dbEntity.Answers,
                entity.Answers,
                (dbE, e) => dbE.Id != 0 && dbE.Id == e.Id,
                answer =>
                {
                    var dbAnswer = new ReadingQuestionAnswer();

                    CopyAnswerFields(dbAnswer, answer);

                    return dbAnswer;
                },
                (dbAnswer, answer) =>
                {
                    CopyAnswerFields(dbAnswer, answer);
                });

            await Task.CompletedTask;
        }

        private void CopyAnswerFields(ReadingQuestionAnswer dst, ReadingQuestionAnswer src)
        {
            dst.Text = src.Text;
            dst.ImageId = src.ImageId;
            dst.IsCorrect = src.IsCorrect;
            dst.Ordinal = src.Ordinal;
        }

        protected override IQueryable<ReadingQuestion> ApplySearchQueryFilter(IQueryable<ReadingQuestion> query, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return query;
            }

            return query.Where(x => x.QuestionText.Contains(searchQuery));
        }

        protected override Expression<Func<ReadingQuestion, object>> MapListResult()
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

        protected override async Task Delete(ReadingQuestion dbEntity)
        {
            foreach (var answer in dbEntity.Answers)
            {
                this.DbContext().Set<ReadingQuestionAnswer>().Remove(answer);
            }

            await base.Delete(dbEntity);
        }

        protected override GridDefinition<ReadingQuestion, ReadingQuestionGridModel> GetGridDefinition()
        {
            return new GridDefinition<ReadingQuestion, ReadingQuestionGridModel>
            {
                Columns = new List<GridColumnDefinition<ReadingQuestionGridModel>>
                {
                    new GridColumnDefinition<ReadingQuestionGridModel>
                    {
                        Title = "Question text",
                        Property = x => x.QuestionText
                    },
                    new GridColumnDefinition<ReadingQuestionGridModel>
                    {
                        Title = "Author",
                        Property = x => x.Author
                    },
                    new GridColumnDefinition<ReadingQuestionGridModel>
                    {
                        Title = "Organization",
                        Property = x => x.Organization
                    },
                },
                Filters = new List<GridFilterDefinition<ReadingQuestion>>
                {
                    new GridFilterDefinition<ReadingQuestion>
                    {
                        Title = "Question text",
                        Property = x => x.Text
                    },
                    new GridFilterDefinition<ReadingQuestion>
                    {
                        Title = "Author",
                        Property = x => x.User
                    },
                    new GridFilterDefinition<ReadingQuestion>
                    {
                        Title = "Organization",
                        Property = x => x.Tenant
                    },
                },
                EntityModelMapperFn = x => new ReadingQuestionGridModel
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