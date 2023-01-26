using Augur.Data.DbContexts;
using Augur.Data.DbMaps.Shared;
using Ngpt.Data.DbMaps;
using Ngpt.Platform.Data.Entities;
using Ngpt.Platform.Data.Entities.AuthTokens;
using Ngpt.Platform.Data.Entities.ChangePasswordRequests;
using Ngpt.Platform.Data.Entities.EmailVerificationRequests;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.UploadedResources;
using Ngpt.Platform.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Ngpt.Data.DbMaps.Questions;
using Ngpt.Data.DbMaps.Questions.DragDrop;
using Ngpt.Data.DbMaps.Questions.Listening;
using Ngpt.Data.DbMaps.Questions.MultipleChoice;
using Ngpt.Data.DbMaps.Questions.Reading;
using Ngpt.Data.DbMaps.Questions.SingleGap;
using Ngpt.Data.DbMaps.Questions.SingleAnswer;

namespace Ngpt.Data.DbContexts
{
    public class RootDbContext : AugurExtendableDbContext
    {
        public RootDbContext(DbContextOptions<RootDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ApplyConfiguration(modelBuilder, new AuthTokenDbMap());
            this.ApplyConfiguration(modelBuilder, new UserDbMap());
            this.ApplyConfiguration(modelBuilder, new SimpleDbMap<SentEmail>("SentEmails"));
            this.ApplyConfiguration(modelBuilder, new ChangePasswordRequestDbMap());
            this.ApplyConfiguration(modelBuilder, new SimpleDbMap<GlobalSettings>("GlobalSettings"));
            this.ApplyConfiguration(modelBuilder, new EmailVerificationRequestDbMap());
            this.ApplyConfiguration(modelBuilder, new AccountInfoDbMap());
            this.ApplyConfiguration(modelBuilder, new TenantDbMap());
            this.ApplyConfiguration(modelBuilder, new UploadedResourceDbMap());

            this.ApplyConfiguration(modelBuilder, new DragDropQuestionDbMap());
            this.ApplyConfiguration(modelBuilder, new DragDropQuestionPartDbMap());
            this.ApplyConfiguration(modelBuilder, new ListeningQuestionAnswerDbMap());
            this.ApplyConfiguration(modelBuilder, new ListeningQuestionAudioDbMap());
            this.ApplyConfiguration(modelBuilder, new ListeningQuestionDbMap());
            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionAnswerPartDbMap());
            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionDbMap());
            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionPartDbMap());
            this.ApplyConfiguration(modelBuilder, new MultipleChoiceQuestionTextPartDbMap());
            this.ApplyConfiguration(modelBuilder, new MutlipleChoiceQuestionAnswerPartOptionDbMap());
            this.ApplyConfiguration(modelBuilder, new ReadingQuestionAnswerDbMap());
            this.ApplyConfiguration(modelBuilder, new ReadingQuestionDbMap());
            this.ApplyConfiguration(modelBuilder, new ReadingQuestionTextDbMap());
            this.ApplyConfiguration(modelBuilder, new SingleGapQuestionDbMap());
            this.ApplyConfiguration(modelBuilder, new SingleGapQuestionAnswerDbMap());
            this.ApplyConfiguration(modelBuilder, new UseOfLanguageQuestionDbMap());
            this.ApplyConfiguration(modelBuilder, new LevelDbMap());
            this.ApplyConfiguration(modelBuilder, new LanguageDbMap());
            this.ApplyConfiguration(modelBuilder, new InstructionDbMap());
            this.ApplyConfiguration(modelBuilder, new SingleAnswerQuestionDbMap());
            this.ApplyConfiguration(modelBuilder, new SingleAnswerQuestionAnswerDbMap());

            this.ApplyConfiguration(modelBuilder, new PlacementTestDbMap());
            this.ApplyConfiguration(modelBuilder, new PlacementTestQuestionDbMap());
            this.ApplyConfiguration(modelBuilder, new PlacementTestInvitationDbMap());

            //this.ApplyConfiguration(modelBuilder, new PermissionOverrideDbMap());
            //this.ApplyConfiguration(modelBuilder, new RoleDbMap());
            //this.ApplyConfiguration(modelBuilder, new SimpleDbMap<DefaultPermission>("DefaultPermissions"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
