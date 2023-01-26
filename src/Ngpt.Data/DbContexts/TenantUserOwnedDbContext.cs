using Augur.Data.DbContextAddons;
using Augur.Data.DbContexts;
using Augur.Data.Extensions;
using Ngpt.Data.DbMaps;
using Ngpt.Platform.Data.Entities.AuthTokens;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbMaps.Questions;
using Ngpt.Data.DbMaps.Questions.DragDrop;
using Ngpt.Data.DbMaps.Questions.Listening;
using Ngpt.Data.DbMaps.Questions.MultipleChoice;
using Ngpt.Data.DbMaps.Questions.Reading;
using Ngpt.Data.DbMaps.Questions.SingleGap;
using Ngpt.Platform.Data.Entities.UploadedResources;
using Ngpt.Data.DbMaps.Questions.SingleAnswer;

namespace Ngpt.Data.DbContexts
{
    public class TenantUserOwnedDbContext : AugurExtendableDbContext
    {
        private readonly ILoggedInTenantIdProvider loggedInTenantIdProvider;
        private readonly ILoggedInUserIdProvider loggedInUserIdProvider;

        public TenantUserOwnedDbContext(DbContextOptions<TenantUserOwnedDbContext> options,
            AugurMultiTenancyDbContextAddon multiTenancyDbContextAddOn,
            AugurUserOwnershipDbContextAddon userOwnershipDbContextAddOn,
            AugurChangeTrackingDbContextAddon changeTrackingDbContextOverride,
            ILoggedInTenantIdProvider loggedInTenantIdProvider,
            ILoggedInUserIdProvider loggedInUserIdProvider)
                : base(options, multiTenancyDbContextAddOn, userOwnershipDbContextAddOn, changeTrackingDbContextOverride)
        {
            this.loggedInTenantIdProvider = loggedInTenantIdProvider;
            this.loggedInUserIdProvider = loggedInUserIdProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ApplyConfiguration(modelBuilder, new UploadedResourceDbMap());

            this.ApplyConfiguration(modelBuilder, new TenantDbMap()
                .WithQueryFilters(x => x.Id == this.loggedInTenantIdProvider.TenantId));

            this.ApplyConfiguration(modelBuilder, new UserDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.Id == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new AuthTokenDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new AccountInfoDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new DragDropQuestionDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new DragDropQuestionPartDbMap());

            this.ApplyConfiguration(modelBuilder, new ListeningQuestionDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new ListeningQuestionAnswerDbMap());

            this.ApplyConfiguration(modelBuilder, new ListeningQuestionAudioDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionAnswerPartDbMap());

            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionPartDbMap());

            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionTextPartDbMap());

            this.ApplyConfiguration(modelBuilder, new MutlipleChoiceQuestionAnswerPartOptionDbMap());

            this.ApplyConfiguration(modelBuilder, new ReadingQuestionAnswerDbMap());

            this.ApplyConfiguration(modelBuilder, new ReadingQuestionDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new ReadingQuestionTextDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new SingleGapQuestionDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new SingleGapQuestionAnswerDbMap());

            this.ApplyConfiguration(modelBuilder, new UseOfLanguageQuestionDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new SingleAnswerQuestionDbMap()
                .WithQueryFilters(x => x.TenantId == this.loggedInTenantIdProvider.TenantId)
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new SingleAnswerQuestionAnswerDbMap());

            this.ApplyConfiguration(modelBuilder, new LevelDbMap());
            this.ApplyConfiguration(modelBuilder, new LanguageDbMap());
            this.ApplyConfiguration(modelBuilder, new InstructionDbMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
