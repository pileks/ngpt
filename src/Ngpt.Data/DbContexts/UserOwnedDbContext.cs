using Augur.Data.DbContextAddons;
using Augur.Data.DbContexts;
using Augur.Data.Extensions;
using Ngpt.Data.DbMaps;
using Ngpt.Platform.Data.Entities.AuthTokens;
using Ngpt.Platform.Data.Entities.ChangePasswordRequests;
using Ngpt.Platform.Data.Entities.EmailVerificationRequests;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
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
    public class UserOwnedDbContext : AugurExtendableDbContext
    {
        private readonly ILoggedInUserIdProvider loggedInUserIdProvider;

        public UserOwnedDbContext(DbContextOptions<UserOwnedDbContext> options,
            AugurUserOwnershipDbContextAddon userOwnershipDbContextAddOn,
            AugurChangeTrackingDbContextAddon changeTrackingDbContextOverride,
            ILoggedInUserIdProvider loggedInUserIdProvider)
                : base(options, userOwnershipDbContextAddOn, changeTrackingDbContextOverride)
        {
            this.loggedInUserIdProvider = loggedInUserIdProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ApplyConfiguration(modelBuilder, new UploadedResourceDbMap());
            
            this.ApplyConfiguration(modelBuilder, new TenantDbMap());

            this.ApplyConfiguration(modelBuilder, new UserDbMap()
                .WithQueryFilters(x => x.Id == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new AuthTokenDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new AccountInfoDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new ChangePasswordRequestDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new EmailVerificationRequestDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));


            this.ApplyConfiguration(modelBuilder, new DragDropQuestionDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new DragDropQuestionPartDbMap());

            this.ApplyConfiguration(modelBuilder, new ListeningQuestionDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new ListeningQuestionAnswerDbMap());

            this.ApplyConfiguration(modelBuilder, new ListeningQuestionAudioDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionAnswerPartDbMap());

            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionPartDbMap());

            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionTextPartDbMap());

            this.ApplyConfiguration(modelBuilder, new MutlipleChoiceQuestionAnswerPartOptionDbMap());

            this.ApplyConfiguration(modelBuilder, new ReadingQuestionAnswerDbMap());

            this.ApplyConfiguration(modelBuilder, new ReadingQuestionDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new ReadingQuestionTextDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new SingleGapQuestionDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new SingleGapQuestionAnswerDbMap());

            this.ApplyConfiguration(modelBuilder, new UseOfLanguageQuestionDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new SingleAnswerQuestionDbMap()
                .WithQueryFilters(x => x.UserId == this.loggedInUserIdProvider.UserId));

            this.ApplyConfiguration(modelBuilder, new SingleAnswerQuestionAnswerDbMap());

            this.ApplyConfiguration(modelBuilder, new LevelDbMap());
            this.ApplyConfiguration(modelBuilder, new LanguageDbMap());
            this.ApplyConfiguration(modelBuilder, new InstructionDbMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
