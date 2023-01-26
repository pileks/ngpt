using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Augur.Emails.Settings;
using Augur.Web;
using Augur.Web.Controllers;
using Augur.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ngpt.Data.DbContexts;
using Ngpt.Platform.Data.Entities.AccountInfos;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Identity.Web.Authorization;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;
using Ngpt.Platform.Services;
using Ngpt.Web.Controllers.OrganizationUsers.Models;
using Ngpt.Web.EmailTemplates.OrganizationUserInvitation;

namespace Ngpt.Web.Controllers.OrganizationUsers
{
    [RequireOrgAdminLoggedIn]
    public class OrganizationUsersController : AugurApiController
    {
        private readonly TenantOwnedDbContext tenantDbContext;
        private readonly RootDbContext rootDbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly EmailSender<RootDbContext> emailSender;
        private readonly EmailSettings emailSettings;
        private readonly IConfiguration configuration;
        private readonly ILoggedInTenantIdProvider loggedInTenantIdProvider;

        public OrganizationUsersController(TenantOwnedDbContext tenantDbContext, RootDbContext rootDbContext, IPasswordHasher<User> passwordHasher, 
            EmailSender<RootDbContext> emailSender, EmailSettings emailSettings, IConfiguration configuration, ILoggedInTenantIdProvider loggedInTenantIdProvider)
        {
            this.tenantDbContext = tenantDbContext;
            this.rootDbContext = rootDbContext;
            this.passwordHasher = passwordHasher;
            this.emailSender = emailSender;
            this.emailSettings = emailSettings;
            this.configuration = configuration;
            this.loggedInTenantIdProvider = loggedInTenantIdProvider;
        }

        [HttpPost("create")]
        public virtual async Task<ActionResult<OrganizationUserModel>> Create([FromBody]OrganizationUserModel entity)
        {
            var userWithSameEmail = await this.rootDbContext.Set<User>().SingleOrDefaultAsync(u => u.Email == entity.Email);

            if (userWithSameEmail != null)
            {
                throw new Exception("Email is taken!");
            }

            var user = new User
            {
                Email = entity.Email,
                IsActive = true,
                IsOrgAdmin = entity.IsOrgAdmin,
                AccountInfo = new AccountInfo
                {
                    Name = entity.Name,
                    HasAcceptedTermsAndPrivacyPolicy = true
                }
            };

            user.Password = this.passwordHasher.HashPassword(user, entity.Password);

            this.tenantDbContext.Set<User>().Add(user);

            await this.tenantDbContext.SaveChangesAsync();

            var applicationUrl = this.configuration.GetValue<string>("ApiUrl");

            var tenant = await this.tenantDbContext.Set<Tenant>().FindAsync(this.loggedInTenantIdProvider.TenantId);

            var emailConfirmationTemplate = new OrganizationUserInvitationTemplate(applicationUrl, this.emailSettings.LogoUrl, user.AccountInfo.Name, user.Email, entity.Password, tenant.DisplayName);

            await this.emailSender.SendWithCommit
            (
                x => x
                    .CreateEmail(emailConfirmationTemplate.Subject, emailConfirmationTemplate.PlainTextBody, emailConfirmationTemplate.HtmlBody)
                    .NoAttachments()
                    .FromSystemAdmin()
                    .AddRecipient(user.Email, user.AccountInfo.Name)
                    .Build()
            );

            return this.Ok(new OrganizationUserModel
            {
                Id = user.Id,
                Email = user.Email,
                IsOrgAdmin = user.IsOrgAdmin,
                Name = user.AccountInfo.Name
            });
        }

        [HttpPost("update")]
        public virtual async Task<ActionResult<OrganizationUserModel>> Update([FromBody] OrganizationUserModel entity)
        {
            var dbEntity = await this.tenantDbContext.Set<User>()
                .Include(x => x.AccountInfo)
                .SingleOrDefaultAsync(x => x.Id == entity.Id);

            dbEntity.Email = entity.Email;
            dbEntity.IsOrgAdmin = entity.IsOrgAdmin;
            dbEntity.AccountInfo.Name = entity.Name;

            if (!string.IsNullOrWhiteSpace(entity.Password))
            {
                dbEntity.Password = this.passwordHasher.HashPassword(dbEntity, entity.Password);
            }

            await this.tenantDbContext.SaveChangesAsync();

            return this.Ok(new OrganizationUserModel
            {
                Id = dbEntity.Id,
                Email = dbEntity.Email,
                IsOrgAdmin = dbEntity.IsOrgAdmin,
                Name = dbEntity.AccountInfo.Name
            });
        }


        [HttpGet("get/{id}")]
        public virtual async Task<ActionResult<OrganizationUserModel>> Get(int id)
        {
            var dbEntity = await this.tenantDbContext.Set<User>()
                .Include(x => x.AccountInfo)
                .SingleOrDefaultAsync(x => x.Id == id);

            return this.Ok(new OrganizationUserModel
            {
                Id = dbEntity.Id,
                Email = dbEntity.Email,
                IsOrgAdmin = dbEntity.IsOrgAdmin,
                Name = dbEntity.AccountInfo.Name
            });
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var entity = await this.tenantDbContext.Set<User>()
                .Include(x => x.AccountInfo)
                .SingleOrDefaultAsync(x => x.Id == id);


            if (entity == null)
            {
                return NotFound();
            }

            this.tenantDbContext.Set<User>().Remove(entity);
            
            await this.tenantDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost(nameof(List))]
        [ExportToFrontendWithCustomHeaders]
        public virtual async Task<ActionResult<IEnumerable<object>>> List(PagingQueryParameters pagingParameters,
            [FromBody] IColumnFilter columnFilter)
        {
            return await GetList(this.tenantDbContext.Set<User>(), pagingParameters, columnFilter, this.MapListResult());
        }

        protected Expression<Func<User, object>> MapListResult()
        {
            return entity => new
            {
                entity.Id,
                entity.Email,
                entity.AccountInfo.Name,
                entity.IsActive,
                entity.IsOrgAdmin
            };
        }

        protected IQueryable<User> ApplySearchQueryFilter(IQueryable<User> query, string searchQuery)
        {
            return string.IsNullOrEmpty(searchQuery)
                ? query
                : query.Where(x => x.Email.Contains(searchQuery));

        }

        protected async Task<ActionResult<IEnumerable<TModel>>> GetList<TModel>(IQueryable<User> entities,
            PagingQueryParameters pagingParameters, IColumnFilter columnFilter,
            Expression<Func<User, TModel>> mapListResultExpression)
        {
            var allEntities =
                this.ApplySearchQueryFilter
                (
                    entities,
                    pagingParameters.SearchQuery
                )
                .Select(mapListResultExpression);

            var pagedList = await this.BuildPagedList(allEntities, pagingParameters);

            return this.Ok(pagedList);
        }


        protected async Task<PagedList<T>> BuildPagedList<T>(IQueryable<T> query,
            PagingQueryParameters pagingParameters)
        {
            var pagedList =
                await PagedList<T>.CreateAsync(query, pagingParameters.PageNumber, pagingParameters.PageSize);

            var previousPageLink = pagedList.HasPrevious
                ? this.CreateEntitiesResourceUri(pagingParameters,
                    ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = pagedList.HasNext
                ? this.CreateEntitiesResourceUri(pagingParameters,
                    ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = pagedList.TotalCount,
                pageSize = pagedList.PageSize,
                currentPage = pagedList.CurrentPage,
                totalPages = pagedList.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            this.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return pagedList;
        }

        protected string CreateEntitiesResourceUri(PagingQueryParameters pagingParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return this.Url.Action(nameof(this.GetList),
                        null,
                        new
                        {
                            searchQuery = pagingParameters.SearchQuery,
                            pageNumber = pagingParameters.PageNumber - 1,
                            pageSize = pagingParameters.PageSize
                        }, this.Request.Scheme);
                case ResourceUriType.NextPage:
                    return this.Url.Action(nameof(this.GetList),
                        null,
                        new
                        {
                            searchQuery = pagingParameters.SearchQuery,
                            pageNumber = pagingParameters.PageNumber + 1,
                            pageSize = pagingParameters.PageSize
                        }, this.Request.Scheme);

                default:
                    return this.Url.Action(nameof(this.GetList),
                        null,
                        new
                        {
                            searchQuery = pagingParameters.SearchQuery,
                            pageNumber = pagingParameters.PageNumber,
                            pageSize = pagingParameters.PageSize
                        }, this.Request.Scheme);
            }
        }
    }
}
