using Augur.Emails.Settings;
using Augur.Web.Controllers;
using Augur.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ngpt.Data.DbContexts;
using Ngpt.Data.Entities;
using Ngpt.Data.Entities.Questions;
using Ngpt.Data.Entities.Questions.Listening;
using Ngpt.Data.Entities.Questions.Reading;
using Ngpt.Platform.Services;
using Ngpt.Web.Controllers.PlacementTests.Models;
using Ngpt.Web.EmailTemplates.OrganizationUserInvitation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ngpt.Web.Controllers.PlacementTests
{
    public class PlacementTestsController : AugurApiController
    {
        private readonly RootDbContext rootDbContext;
        private readonly IConfiguration configuration;
        private readonly EmailSender<RootDbContext> emailSender;
        private readonly EmailSettings emailSettings;
        public PlacementTestsController(
            RootDbContext rootDbContext,
            IConfiguration configuration,
            EmailSender<RootDbContext> emailSender,
            EmailSettings emailSettings)
        {
            this.rootDbContext = rootDbContext;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.emailSettings = emailSettings;
        }

        [HttpPost(nameof(Invite))]
        public async Task<ValidationResult> Invite([FromBody] PlacementTestInviteModel model)
        {
            var invitation = new PlacementTestInvitation
            {
                Email = model.Email,
                HasGivenMarketingPermission = model.HasGivenMarketingPermission,
                LanguageId = model.LanguageId,
                Token = Guid.NewGuid().ToString("N")
            };

            await this.rootDbContext.Set<PlacementTestInvitation>().AddAsync(invitation);
            await this.rootDbContext.SaveChangesAsync();

            var applicationUrl = this.configuration.GetValue<string>("ApiUrl");

            var placementTestInvitationTemplate = new PlacementTestInvitationTemplate(
                applicationUrl,
                this.emailSettings.LogoUrl,
                invitation.Token
            );

            await this.emailSender.SendWithCommit
            (
                x => x
                    .CreateEmail(placementTestInvitationTemplate.Subject, placementTestInvitationTemplate.PlainTextBody, placementTestInvitationTemplate.HtmlBody)
                    .NoAttachments()
                    .FromSystemAdmin()
                    .AddRecipient(invitation.Email, invitation.Email)
                    .Build()
            );

            return Ok();
        }

        [HttpGet(nameof(GetInvitationByToken))]
        public async Task<ValidationResult<PlacementTestInvitation>> GetInvitationByToken(string token)
        {
            var invitation = await this.rootDbContext.Set<PlacementTestInvitation>().SingleOrDefaultAsync(x => x.Token == token);

            return Ok(invitation);
        }

        [HttpPost(nameof(Start))]
        public async Task<ValidationResult<PlacementTestStartResultModel>> Start(
            int languageId,
            int? reportedLevelId = null,
            int? invitationId = null,
            double? rating = null,
            double? rd = null,
            double? vol = null,
            bool? shouldTestReading = null,
            bool? shouldTestListening = null)
        {
            var placementTest = new PlacementTest
            {
                Rating = rating ?? 800.0,
                Rd = rd ?? 200.0,
                Vol = vol ?? 0.06,
                LanguageId = languageId,
                StartedOn = DateTime.UtcNow,
                ReportedLevelId = reportedLevelId,
                ShouldTestReading = shouldTestReading ?? false,
                ShouldTestListening = shouldTestListening ?? false
            };

            await rootDbContext.Set<PlacementTest>().AddAsync(placementTest);
            await rootDbContext.SaveChangesAsync();

            if (invitationId.HasValue)
            {
                var invitation = await rootDbContext.Set<PlacementTestInvitation>().SingleOrDefaultAsync(x => x.Id == invitationId);
                invitation.PlacementTestId = placementTest.Id;
                await rootDbContext.SaveChangesAsync();
            }

            return Ok(new PlacementTestStartResultModel
            {
                PlacementTest = placementTest
            });
        }

        [HttpGet(nameof(Get))]
        public async Task<ValidationResult<PlacementTest>> Get(int id)
        {
            var placementTest = await rootDbContext.Set<PlacementTest>().SingleOrDefaultAsync(x => x.Id == id);

            if (placementTest == null)
            {
                return NotFound();
            }

            return Ok(placementTest);
        }

        [HttpGet(nameof(GetQuestionsWithinRating))]
        public async Task<ValidationResult<PlacementTestQuestionsModel>> GetQuestionsWithinRating(double rating, double rd, int languageId, int count, int? placemetTestId = null)
        {
            var fromRating = rating - rd * 2;
            var toRating = rating + rd * 2;

            var questions = new List<UseOfLanguageQuestion>();
            var alreadyAnsweredQuestions = await rootDbContext.Set<PlacementTestQuestion>()
                .Where(x => x.PlacementTestId == placemetTestId)
                .Select(x => x.QuestionId)
                .ToListAsync();

            for (int i = 0; i < count; i++)
            {
                var alreadySelectedQuestionIds = questions.Select(x => x.Id).ToList();
                var excludeQuestionIds = alreadyAnsweredQuestions.Concat(alreadySelectedQuestionIds);

                var question = await this.UseOfLanguageQuestionsQuery()
                    .Where(x => x.LanguageId == languageId)
                    .Where(x => fromRating <= x.Level.Rating)
                    .Where(x => x.Level.Rating <= toRating)
                    .Where(x => !excludeQuestionIds.Any(e => e == x.Id))
                    .Where(x => x.Approved)
                    .OrderBy(r => Guid.NewGuid())
                    .FirstOrDefaultAsync();

                if (question != null)
                {
                    questions.Add(question);
                }
            }

            return Ok(new PlacementTestQuestionsModel
            {
                Questions = questions
            });
        }

        [HttpPost(nameof(UpdateProgress))]
        public async Task<ValidationResult> UpdateProgress([FromBody] PlacementTestProgressModel model)
        {
            var placementTest = await this.rootDbContext.Set<PlacementTest>().SingleOrDefaultAsync(x => x.Id == model.PlacementTestId);

            var questionsAnswered = model.Questions.Select(question => new PlacementTestQuestion
            {
                PlacementTestId = model.PlacementTestId,
                Rating = model.Rating,
                Rd = model.Rd,
                Vol = model.Vol,

                QuestionId = question.QuestionId,
                IsAnsweredCorrectly = question.IsAnsweredCorrectly
            });

            placementTest.Rating = model.Rating;
            placementTest.Rd = model.Rd;
            placementTest.Vol = model.Vol;

            if (model.IsCompleted)
            {
                placementTest.CompletedOn = DateTime.UtcNow;
            }

            await this.rootDbContext.Set<PlacementTestQuestion>().AddRangeAsync(questionsAnswered);
            await this.rootDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet(nameof(GetReadingTest))]
        public async Task<ValidationResult<GetReadingTestResultModel>> GetReadingTest(int placementTestId)
        {
            var placementTest = await this.rootDbContext.Set<PlacementTest>()
                .SingleOrDefaultAsync(x => x.Id == placementTestId);

            if (placementTest == null)
            {
                return NotFound();
            }

            var level = await GetLevelForRating(placementTest.Rating);

            var readingText = await this.rootDbContext.Set<ReadingQuestionText>()
                .Where(x => x.Approved)
                .Where(x => x.LanguageId == placementTest.LanguageId)
                .Where(x => x.LevelId == level.Id)
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (readingText == null)
            {
                return Ok(new GetReadingTestResultModel
                {
                    ShouldSkip = true
                });
            }

            var readingQuestions = await this.rootDbContext.Set<ReadingQuestion>()
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Image)
                .Where(x => x.TextId == readingText.Id)
                .OrderBy(x => Guid.NewGuid())
                .Take(5)
                .ToListAsync();

            return Ok(new GetReadingTestResultModel
            {
                Text = readingText,
                Questions = readingQuestions
            });
        }

        [HttpPost(nameof(CompleteReadingTest))]
        public async Task<ValidationResult> CompleteReadingTest([FromBody] CompleteReadingTestRequestModel model)
        {
            var placementTest = await this.rootDbContext.Set<PlacementTest>()
                .SingleOrDefaultAsync(x => x.Id == model.PlacementTestId);

            if (placementTest == null)
            {
                return NotFound();
            }

            placementTest.ReadingTextTotalAnswers = model.TotalAnswers;
            placementTest.ReadingTextCorrectAnswers = model.CorrectAnswers;
            placementTest.ReadingQuestionTextId = model.TextId;

            await this.rootDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet(nameof(GetListeningTest))]
        public async Task<ValidationResult<GetListeningTestResultModel>> GetListeningTest(int placementTestId)
        {
            var placementTest = await this.rootDbContext.Set<PlacementTest>()
                .SingleOrDefaultAsync(x => x.Id == placementTestId);

            if (placementTest == null)
            {
                return NotFound();
            }

            var level = await GetLevelForRating(placementTest.Rating);

            var listeningAudio = await this.rootDbContext.Set<ListeningQuestionAudio>()
                .Where(x => x.Approved)
                .Where(x => x.LanguageId == placementTest.LanguageId)
                .Where(x => x.LevelId == level.Id)
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (listeningAudio == null)
            {
                return Ok(new GetListeningTestResultModel
                {
                    ShouldSkip = true
                });
            }

            var listeningQuestions = await this.rootDbContext.Set<ListeningQuestion>()
                .Include(x => x.Answers)
                    .ThenInclude(x => x.Image)
                .Where(x => x.AudioId == listeningAudio.Id)
                .OrderBy(x => Guid.NewGuid())
                .Take(5)
                .ToListAsync();

            return Ok(new GetListeningTestResultModel
            {
                Audio = listeningAudio,
                Questions = listeningQuestions
            });
        }

        [HttpPost(nameof(CompleteListeningTest))]
        public async Task<ValidationResult> CompleteListeningTest([FromBody] CompleteListeningTestRequestModel model)
        {
            var placementTest = await this.rootDbContext.Set<PlacementTest>()
                .SingleOrDefaultAsync(x => x.Id == model.PlacementTestId);

            if (placementTest == null)
            {
                return NotFound();
            }

            placementTest.ListeningAudioTotalAnswers = model.TotalAnswers;
            placementTest.ListeningAudioCorrectAnswers = model.CorrectAnswers;
            placementTest.ListeningQuestionAudioId = model.AudioId;

            await this.rootDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet(nameof(GetResult))]
        public async Task<ValidationResult<PlacementTestResultModel>> GetResult(int placementTestId)
        {
            var placementTest = await this.rootDbContext.Set<PlacementTest>()
                .SingleOrDefaultAsync(x => x.Id == placementTestId);

            var level = await GetLevelForRating(placementTest.Rating);

            return Ok(new PlacementTestResultModel
            {
                PlacementTest = placementTest,
                Level = level
            });
        }

        private IQueryable<UseOfLanguageQuestion> UseOfLanguageQuestionsQuery()
        {
            return rootDbContext.Set<UseOfLanguageQuestion>()
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
                .Include(x => x.Instruction);
        }

        private async Task<Level> GetLevelForRating(double rating)
        {
            var level = await this.rootDbContext.Set<Level>()
                .SingleOrDefaultAsync(x => x.FromRating <= rating && x.ToRating >= rating);

            // If level is out of normal range, we can use this edge case to get A1L or C2H level
            if (level == null)
            {
                var levels = await this.rootDbContext.Set<Level>()
                    .OrderBy(x => x.Rating)
                    .ToListAsync();

                if (levels.First().FromRating >= rating)
                {
                    level = levels.First();
                }
                else
                {
                    level = levels.Last();
                }
            }

            return level;
        }
    }
}
