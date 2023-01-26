using Augur.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Augur.Emails.EmailBuilding;
using Augur.Emails.Services;
using Microsoft.EntityFrameworkCore;

namespace Ngpt.Platform.Services
{
    public class EmailSender<TDbContext> where TDbContext : DbContext
    {
        private readonly EmailBuilder emailBuilder;
        private readonly TDbContext dbContext;

        public EmailSender(EmailBuilder emailBuilder, TDbContext dbContext)
        {
            this.emailBuilder = emailBuilder;
            this.dbContext = dbContext;
        }

        public async Task<IList<Ngpt.Platform.Data.Entities.SentEmail>> SendWithCommit(Func<EmailBuilder, EmailSender> build)
        {
            var emails = await build(this.emailBuilder).Send();

            var platformEmails = emails.Select(x => new Ngpt.Platform.Data.Entities.SentEmail
            {
                Subject = x.Subject,
                PlainTextBody = x.PlainTextBody,
                HtmlBody = x.HtmlBody,
                From = x.From,
                To = x.To,
                Date = x.Date
            }).ToList();

            this.dbContext.Set<Ngpt.Platform.Data.Entities.SentEmail>().AddRange(platformEmails);

            await this.dbContext.SaveChangesAsync();

            return platformEmails;
        }

        public async Task<IList<Ngpt.Platform.Data.Entities.SentEmail>> SendWithoutCommit(Func<EmailBuilder, EmailSender> build)
        {
            var emails = await build(this.emailBuilder).Send();

            var platformEmails = emails.Select(x => new Ngpt.Platform.Data.Entities.SentEmail
            {
                Subject = x.Subject,
                PlainTextBody = x.PlainTextBody,
                HtmlBody = x.HtmlBody,
                From = x.From,
                To = x.To,
                Date = x.Date
            }).ToList();

            this.dbContext.Set<Ngpt.Platform.Data.Entities.SentEmail>().AddRange(platformEmails);

            return platformEmails;
        }
    }
}