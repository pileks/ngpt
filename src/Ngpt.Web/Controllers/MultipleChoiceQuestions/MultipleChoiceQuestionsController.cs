using System;
using System.Threading.Tasks;
using Augur.ApiSystemEvents;
using Augur.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Entities.Questions.MultipleChoice;
using Ngpt.Platform.Data.Entities.AccountInfos;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Services.Enums;

namespace Ngpt.Web.Controllers.MultipleChoiceQuestions
{
    [RequireUserWithVerifiedEmailLoggedIn]
    public class MultipleChoiceQuestionsController : AugurEntityController<MultipleChoiceQuestion>
    {
        private readonly TenantUserOwnedDbContext tenantUserOwnedDbContext;
        private readonly ILoggedInUserProvider loggedInUserProvider;
        private readonly IHeaderClientNotifier headerClientNotifier;

        public MultipleChoiceQuestionsController(TenantUserOwnedDbContext tenantUserOwnedDbContext, ILoggedInUserProvider loggedInUserProvider,
            IHeaderClientNotifier clientSystemEventNotifier) : base(tenantUserOwnedDbContext)
        {
            this.tenantUserOwnedDbContext = tenantUserOwnedDbContext;
            this.loggedInUserProvider = loggedInUserProvider;
            this.headerClientNotifier = clientSystemEventNotifier;
        }

        protected override async Task<MultipleChoiceQuestion> GetSingleOrDefaultAsync(int id)
        {
            return await this.DbSet()
                .Include(x => x.Parts)
                    .ThenInclude(x => x.AnswerPart)
                        .ThenInclude(x => x.Options)
                .Include(x => x.Parts)
                    .ThenInclude(x => x.TextPart)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        protected override MultipleChoiceQuestion BeforeCreate(MultipleChoiceQuestion entity)
        {
            return base.BeforeCreate(entity);
        }

        protected override async Task Update(MultipleChoiceQuestion entity, MultipleChoiceQuestion dbEntity)
        {
            AddUpdateRemoveEfCollection(dbEntity.Parts, entity.Parts,
                (dbPart, part) => dbPart.Id == part.Id && dbPart.Id != 0,
                part =>
                {
                    var dbPart = part;

                    return dbPart;
                },
                (dbPart, part) =>
                {
                    dbPart.Ordinal = part.Ordinal;
                },
                dbPart =>
                {
                    this.tenantUserOwnedDbContext.Set<MultipleChoiceQuestionPart>().Remove(dbPart);
                });

            await Task.CompletedTask;
        }

        protected override Task Delete(MultipleChoiceQuestion dbEntity)
        {
            throw new InvalidOperationException();
        }
    }
}