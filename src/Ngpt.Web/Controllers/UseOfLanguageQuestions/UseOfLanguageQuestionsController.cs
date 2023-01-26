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
using Ngpt.Data.Entities.Questions;
using Ngpt.Data.Entities.Questions.DragDrop;
using Ngpt.Data.Entities.Questions.MultipleChoice;
using Ngpt.Data.Entities.Questions.SingleAnswer;
using Ngpt.Data.Entities.Questions.SingleGap;
using Ngpt.Data.Enums;
using Ngpt.Data.Services;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;
using Ngpt.Web.Controllers.UseOfLanguageQuestions.Models;
using Ngpt.Web.GenericControllers;

namespace Ngpt.Web.Controllers.UseOfLanguageQuestions
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class UseOfLanguageQuestionsController : ScopedEntitySecurityWithGridController<UseOfLanguageQuestion, UseOfLanguageQuestionGridModel>
    {
        private readonly RootDbContext rootDbContext;
        private readonly TenantUserOwnedDbContext tenantUserOwnedDbContext;
        private readonly ILoggedInUserIdProvider loggedInUserIdProvider;

        public UseOfLanguageQuestionsController(RootDbContext rootDbContext,
            TenantUserOwnedDbContext tenantUserOwnedDbContext,
            ScopedEntitySecurityService scopedEntitySecurityService,
            ILoggedInTenantIdProvider loggedInTenantIdProvider,
            ILoggedInUserIdProvider loggedInUserIdProvider
        )
            : base(tenantUserOwnedDbContext, scopedEntitySecurityService)
        {
            this.rootDbContext = rootDbContext;
            this.tenantUserOwnedDbContext = tenantUserOwnedDbContext;
            this.loggedInUserIdProvider = loggedInUserIdProvider;
        }

        [HttpPost(nameof(ToggleApproval))]
        public async Task<ActionResult<bool>> ToggleApproval([FromBody] UseOfLanguageQuestion question)
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

            await this.TryCommit(new UpdateFailedException(typeof(UseOfLanguageQuestion), dbQuestion.Id.ToString()));

            return this.Ok(dbQuestion.Approved);
        }

        protected override async Task<UseOfLanguageQuestion> GetSingleOrDefaultAsync(int id)
        {
            return await DbSet()
                //Drag-drop
                .Include(x => x.DragDropQuestion)
                    .ThenInclude(x => x.Parts)
                //Multi-choice
                .Include(x => x.MultipleChoiceQuestion)
                    .ThenInclude(x => x.Parts)
                        .ThenInclude(x => x.AnswerPart)
                            .ThenInclude(x => x.Options)
                .Include(x => x.MultipleChoiceQuestion)
                    .ThenInclude(x => x.Parts)
                        .ThenInclude(x => x.TextPart)
                //Single-gap
                .Include(x => x.SingleGapQuestion)
                    .ThenInclude(x => x.Answers)
                //Single-answer
                .Include(x => x.SingleAnswerQuestion)
                    .ThenInclude(x => x.Answers)
                //The rest
                .Include(x => x.Level)
                .Include(x => x.Instruction)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override UseOfLanguageQuestion BeforeCreate(UseOfLanguageQuestion entity)
        {
            //Sanitize entity
            entity.Level = null;
            entity.Language = null;
            entity.Instruction = null;

            if (entity.Type != UseOfLanguageQuestionType.MultipleChoice)
            {
                entity.MultipleChoiceQuestion = null;
            }
            if (entity.Type != UseOfLanguageQuestionType.SingleGap)
            {
                entity.SingleGapQuestion = null;
            }
            if (entity.Type != UseOfLanguageQuestionType.DragDrop)
            {
                entity.DragDropQuestion = null;
            }
            if (entity.Type != UseOfLanguageQuestionType.SingleAnswer)
            {
                entity.SingleAnswerQuestion = null;
            }
            return base.BeforeCreate(entity);
        }

        protected override async Task Update(UseOfLanguageQuestion entity, UseOfLanguageQuestion dbEntity)
        {
            dbEntity.LevelId = entity.LevelId;
            dbEntity.Title = entity.Title;
            dbEntity.LanguageId = entity.LanguageId;
            dbEntity.InstructionId = entity.InstructionId;

            if (dbEntity.Type == UseOfLanguageQuestionType.SingleGap)
            {
                dbEntity.SingleGapQuestion.TextAfter = entity.SingleGapQuestion.TextAfter;
                dbEntity.SingleGapQuestion.TextBefore = entity.SingleGapQuestion.TextBefore;

                AddUpdateRemoveEfCollection(
                    dbEntity.SingleGapQuestion.Answers,
                    entity.SingleGapQuestion.Answers,
                    (dbE, e) => e.Id != 0 && dbE.Id == e.Id,
                    answer => new SingleGapQuestionAnswer
                    {
                        Text = answer.Text,
                        IsCaseSensitive = answer.IsCaseSensitive
                    },
                    (dbAnswer, answer) =>
                    {
                        dbAnswer.Text = answer.Text;
                        dbAnswer.IsCaseSensitive = answer.IsCaseSensitive;
                    });
            }
            else if (dbEntity.Type == UseOfLanguageQuestionType.DragDrop)
            {
                AddUpdateRemoveEfCollection(
                    dbEntity.DragDropQuestion.Parts,
                    entity.DragDropQuestion.Parts,
                    (dbE, e) => e.Id != 0 && dbE.Id == e.Id,
                    part => new DragDropQuestionPart
                    {
                        Text = part.Text,
                        IsDraggable = part.IsDraggable,
                        Ordinal = part.Ordinal,
                    },
                    (dbPart, part) =>
                    {
                        dbPart.Text = part.Text;
                        dbPart.IsDraggable = part.IsDraggable;
                        dbPart.Ordinal = part.Ordinal;
                    });
            }
            else if (dbEntity.Type == UseOfLanguageQuestionType.MultipleChoice)
            {
                AddUpdateRemoveEfCollection(
                    dbEntity.MultipleChoiceQuestion.Parts,
                    entity.MultipleChoiceQuestion.Parts,
                    (dbE, e) => e.Id != 0 && dbE.Id == e.Id,
                    part => part,
                    (dbPart, part) =>
                    {
                        dbPart.Ordinal = part.Ordinal;
                        if (dbPart.TextPart != null)
                        {
                            dbPart.TextPart.Text = part.TextPart.Text;
                        }

                        if (dbPart.AnswerPart != null)
                        {
                            AddUpdateRemoveEfCollection(
                                dbPart.AnswerPart.Options,
                                part.AnswerPart.Options,
                                (dbO, o) => o.Id != null && dbO.Id == o.Id,
                                option => option,
                                (dbOption, option) =>
                                {
                                    dbOption.Text = option.Text;
                                    dbOption.IsCorrect = option.IsCorrect;
                                });
                        }
                    });
            }
            else if (dbEntity.Type == UseOfLanguageQuestionType.SingleAnswer)
            {
                dbEntity.SingleAnswerQuestion.QuestionText = entity.SingleAnswerQuestion.QuestionText;
                dbEntity.SingleAnswerQuestion.AnswerType = entity.SingleAnswerQuestion.AnswerType;

                AddUpdateRemoveEfCollection(
                    dbEntity.SingleAnswerQuestion.Answers,
                    entity.SingleAnswerQuestion.Answers,
                    (dbE, e) => e.Id != 0 && dbE.Id == e.Id,
                    answer => new SingleAnswerQuestionAnswer
                    {
                        Text = answer.Text,
                        ImageId = answer.ImageId,
                        IsCorrect = answer.IsCorrect
                    },
                    (dbAnswer, answer) =>
                    {
                        dbAnswer.Text = answer.Text;
                        dbAnswer.ImageId = answer.ImageId;
                        dbAnswer.IsCorrect = answer.IsCorrect;
                    });
            }
            await Task.CompletedTask;
        }

        protected override IQueryable<UseOfLanguageQuestion> ApplySearchQueryFilter(IQueryable<UseOfLanguageQuestion> query, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return query;
            }

            return query.Where(x => x.Title.Contains(searchQuery));
        }

        protected override Expression<Func<UseOfLanguageQuestion, object>> MapListResult()
        {
            return e => new
            {
                e.Id,
                e.Title,
                e.Approved,
                Language = new
                {
                    Id = e.Language.Id,
                    Name = e.Language.Name
                },
                Level = new
                {
                    Id = e.Level.Id,
                    Title = e.Level.Title
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

        protected override async Task Delete(UseOfLanguageQuestion dbEntity)
        {
            //single gap delete
            if (dbEntity.SingleGapQuestion != null)
            {
                foreach (var answer in dbEntity.SingleGapQuestion.Answers)
                {
                    this.DbContext().Set<SingleGapQuestionAnswer>().Remove(answer);
                }

                this.DbContext().Set<SingleGapQuestion>().Remove(dbEntity.SingleGapQuestion);
            }

            //multiple choice delete
            if (dbEntity.MultipleChoiceQuestion != null)
            {
                foreach (var part in dbEntity.MultipleChoiceQuestion.Parts)
                {
                    if (part.AnswerPart != null)
                    {
                        foreach (var option in part.AnswerPart.Options)
                        {
                            this.DbContext().Set<MutlipleChoiceQuestionAnswerPartOption>().Remove(option);
                        }

                        this.DbContext().Set<MultipleChoiceQuestionAnswerPart>().Remove(part.AnswerPart);
                    }
                    if (part.TextPart != null)
                    {
                        this.DbContext().Set<MultipleChoiceQuestionTextPart>().Remove(part.TextPart);
                    }

                    this.DbContext().Set<MultipleChoiceQuestionPart>().Remove(part);
                }

                this.DbContext().Set<MultipleChoiceQuestion>().Remove(dbEntity.MultipleChoiceQuestion);
            }

            //drag drop delete
            if (dbEntity.DragDropQuestion != null)
            {
                foreach (var part in dbEntity.DragDropQuestion.Parts)
                {
                    this.DbContext().Set<DragDropQuestionPart>().Remove(part);
                }

                this.DbContext().Set<DragDropQuestion>().Remove(dbEntity.DragDropQuestion);
            }

            //single answer delete
            if (dbEntity.SingleAnswerQuestion != null)
            {
                foreach (var answer in dbEntity.SingleAnswerQuestion.Answers)
                {
                    this.DbContext().Set<SingleAnswerQuestionAnswer>().Remove(answer);
                }

                this.DbContext().Set<SingleAnswerQuestion>().Remove(dbEntity.SingleAnswerQuestion);
            }

            await base.Delete(dbEntity);
        }

        protected override GridDefinition<UseOfLanguageQuestion, UseOfLanguageQuestionGridModel> GetGridDefinition()
        {
            return new GridDefinition<UseOfLanguageQuestion, UseOfLanguageQuestionGridModel>
            {
                Columns = new List<GridColumnDefinition<UseOfLanguageQuestionGridModel>>
                {
                    new GridColumnDefinition<UseOfLanguageQuestionGridModel>
                    {
                        Title = "Title",
                        Property = x => x.Title
                    },
                    new GridColumnDefinition<UseOfLanguageQuestionGridModel>
                    {
                        Title = "Language",
                        Property = x => x.Language
                    },
                    new GridColumnDefinition<UseOfLanguageQuestionGridModel>
                    {
                        Title = "Level",
                        Property = x => x.Level
                    },
                    new GridColumnDefinition<UseOfLanguageQuestionGridModel>
                    {
                        Title = "Type",
                        Property = x => x.Type
                    },
                    new GridColumnDefinition<UseOfLanguageQuestionGridModel>
                    {
                        Title = "Author",
                        Property = x => x.Author
                    },
                    new GridColumnDefinition<UseOfLanguageQuestionGridModel>
                    {
                        Title = "Organization",
                        Property = x => x.Organization
                    },
                    new GridColumnDefinition<UseOfLanguageQuestionGridModel>
                    {
                        Title = "Approved",
                        Property = x => x.Approved
                    },
                    new GridColumnDefinition<UseOfLanguageQuestionGridModel>
                    {
                        Title = "Approved by",
                        Property = x => x.ApprovedBy
                    },
                },
                Filters = new List<GridFilterDefinition<UseOfLanguageQuestion>>
                {
                    new GridFilterDefinition<UseOfLanguageQuestion>
                    {
                        Title = "Title",
                        Property = x => x.Title
                    },
                    new GridFilterDefinition<UseOfLanguageQuestion>
                    {
                        Title = "Language",
                        Property = x => x.Language
                    },
                    new GridFilterDefinition<UseOfLanguageQuestion>
                    {
                        Title = "Level",
                        Property = x => x.Level
                    },
                    new GridFilterDefinition<UseOfLanguageQuestion>
                    {
                        Title = "Type",
                        Property = x => x.Type
                    },
                    new GridFilterDefinition<UseOfLanguageQuestion>
                    {
                        Title = "Author",
                        Property = x => x.User
                    },
                    new GridFilterDefinition<UseOfLanguageQuestion>
                    {
                        Title = "Organization",
                        Property = x => x.Tenant
                    },
                    new GridFilterDefinition<UseOfLanguageQuestion>
                    {
                        Title = "Approved",
                        Property = x => x.Approved
                    },
                    new GridFilterDefinition<UseOfLanguageQuestion>
                    {
                        Title = "Approved by",
                        Property = x => x.Approver
                    },
                },
                EntityModelMapperFn = x => new UseOfLanguageQuestionGridModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Language = x.Language.Name,
                    Level = x.Level.Title,
                    Type = x.Type,
                    Author = x.User.AccountInfo.Name,
                    Organization = x.Tenant.Name,
                    Approved = x.Approved,
                    ApprovedBy = x.Approver.AccountInfo.Name
                }
            };
        }
    }
}