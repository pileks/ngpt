using System;
using System.Collections.Generic;
using System.Linq;
using Augur.Web.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Augur.Web.Helpers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Augur.Entity.Interfaces.Base;
using Augur.Data.Extensions;

namespace Augur.Web.Controllers
{
    public abstract class AugurEntityController<TEntity> : AugurEntityController<TEntity, int, IColumnFilter>
        where TEntity : class, IAugurEntityWithId
    {
        protected AugurEntityController(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public abstract class AugurEntityController<TEntity, TId> : AugurEntityController<TEntity, int, IColumnFilter>
        where TEntity : class, IAugurEntityWithId
    {
        protected AugurEntityController(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public abstract class AugurEntityController<TEntity, TId, TColumnFilter> : AugurApiController
        where TEntity : class, IAugurEntityWithId<TId> where TColumnFilter : class, IColumnFilter
    {
        private readonly DbContext dbContext;

        protected AugurEntityController(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TEntity entity)
        {
            if (entity == null)
            {
                return this.BadRequest();
            }

            if (!this.TryValidateModel(entity))
            {
                return new UnprocessableEntityObjectResult(this.ModelState);
            }

            var processedEntity = this.BeforeCreate(entity);

            this.DbContext().Set<TEntity>().Add(processedEntity);

            await this.TryCommit(new CreateFailedException(typeof(TEntity)));

            this.AfterCreate(processedEntity);
            return this.CreatedAtAction
            (
                nameof(this.Get),
                new {id = processedEntity.Id},
                processedEntity
            );
        }

        protected virtual TEntity BeforeCreate(TEntity entity)
        {
            return entity;
        }

        protected virtual Task AfterCreate(TEntity dbEntity)
        {
            return Task.CompletedTask;
        }

        [HttpPost("{id}")]
        public virtual async Task<IActionResult> BlockCreation(TId id)
        {
            if (await this.DbSet().AnyAsync(id))
            {
                return this.Conflict();
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(TId id)
        {
            var entity = await this.GetSingleOrDefaultAsync(id);

            if (entity == null)
            {
                return this.NotFound();
            }

            var processedEntity = await AfterGet(entity);

            return this.Ok(processedEntity);
        }

        protected virtual Task<TEntity> AfterGet(TEntity entity)
        {
            return Task.FromResult(entity);
        }

        protected virtual async Task<TEntity> GetSingleOrDefaultAsync(TId id)
        {
            return await this.DbSet().FindAsync(id);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(TId id, [FromBody] TEntity entity)
        {
            if (entity == null)
            {
                return this.BadRequest();
            }

            if (!this.TryValidateModel(entity))
            {
                return new UnprocessableEntityObjectResult(this.ModelState);
            }

            var dbEntity = await this.GetSingleOrDefaultAsync(id);

            if (dbEntity == null)
            {
                return this.NotFound();
            }

            entity.Id = id;

            await this.Update(entity, dbEntity);

            await this.TryCommit(new UpdateFailedException(typeof(TEntity), id.ToString()));

            this.AfterUpdate(entity, dbEntity);

            return this.NoContent();
        }

        protected virtual void AfterUpdate(TEntity entity, TEntity dbEntity)
        {
        }

        protected virtual async Task Update(TEntity entity, TEntity dbEntity)
        {
            await Task.CompletedTask;
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TId id)
        {
            var entity = await this.GetSingleOrDefaultAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            await this.Delete(entity);

            await TryCommit(new DeleteFailedException(typeof(TEntity)));

            await AfterDelete(entity);

            return NoContent();
        }

        protected virtual async Task Delete(TEntity dbEntity)
        {
            this.DbSet().Remove(dbEntity);

            await Task.CompletedTask;
        }

        protected virtual async Task AfterDelete(TEntity dbEntity)
        {
            await Task.CompletedTask;
        }

        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> PartiallyUpdate(TId id, [FromBody] JsonPatchDocument<TEntity> patchDoc)
        {
            if (patchDoc == null)
            {
                return this.BadRequest();
            }

            var entity = await this.DbSet().FindAsync(id);

            if (entity == null)
            {
                return this.NotFound();
            }

            patchDoc.ApplyTo(entity);

            entity.Id = id;

            await this.TryCommit(new PatchFailedException(typeof(TEntity), id.ToString()));

            return this.Ok(entity);
        }

        [HttpPost(nameof(List))]
        public virtual async Task<ActionResult<IEnumerable<object>>> List(PagingQueryParameters pagingParameters,
            [FromBody] TColumnFilter columnFilter)
        {
            return await GetList(this.Query(), pagingParameters, columnFilter, this.MapListResult());
        }

        protected async Task<ActionResult<IEnumerable<TModel>>> GetList<TModel>(IQueryable<TEntity> entities,
            PagingQueryParameters pagingParameters, TColumnFilter columnFilter,
            Expression<Func<TEntity, TModel>> mapListResultExpression)
        {
            var allEntities =
                this.ApplyOrdering
                    (
                        this.ApplySearchQueryFilter
                        (
                            this.ApplyColumnFilter(entities, columnFilter),
                            pagingParameters.SearchQuery
                        )
                    )
                    .Select(mapListResultExpression);

            var pagedList = await this.BuildPagedList(allEntities, pagingParameters);

            return this.Ok(pagedList);
        }

        protected virtual IQueryable<TEntity> ApplyColumnFilter(IQueryable<TEntity> query, TColumnFilter columnFilter)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> ApplySearchQueryFilter(IQueryable<TEntity> query, string searchQuery)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> query)
        {
            return query;
        }

        protected List<string> GetSearchQueryPartsLowered(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return new List<string>() { };
            }

            return searchQuery
                .Substring(0, Math.Min(300, searchQuery.Length))
                .ToLower()
                .Trim()
                .Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
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

        protected virtual Expression<Func<TEntity, object>> MapListResult()
        {
            return e => e;
        }

        [HttpGet(nameof(GetAll))]
        public virtual async Task<IActionResult> GetAll()
        {
            var allEntities = await this.Query()
                .OrderBy(x => x.Id)
                .Select(this.MapGetAllResult())
                .ToListAsync();

            return this.Ok(allEntities);
        }

        protected virtual Expression<Func<TEntity, object>> MapGetAllResult()
        {
            return e => e;
        }

        protected string CreateEntitiesResourceUri(PagingQueryParameters pagingParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return this.Url.Action(nameof(this.GetAll),
                        null,
                        new
                        {
                            searchQuery = pagingParameters.SearchQuery,
                            pageNumber = pagingParameters.PageNumber - 1,
                            pageSize = pagingParameters.PageSize
                        }, this.Request.Scheme);
                case ResourceUriType.NextPage:
                    return this.Url.Action(nameof(this.GetAll),
                        null,
                        new
                        {
                            searchQuery = pagingParameters.SearchQuery,
                            pageNumber = pagingParameters.PageNumber + 1,
                            pageSize = pagingParameters.PageSize
                        }, this.Request.Scheme);

                default:
                    return this.Url.Action(nameof(this.GetAll),
                        null,
                        new
                        {
                            searchQuery = pagingParameters.SearchQuery,
                            pageNumber = pagingParameters.PageNumber,
                            pageSize = pagingParameters.PageSize
                        }, this.Request.Scheme);
            }
        }

        protected async Task TryCommit(Exception exception)
        {
            await this.DbContext().SaveChangesAsync();
        }

        protected virtual DbSet<TEntity> DbSet()
        {
            return this.DbContext().Set<TEntity>();
        }

        protected virtual DbContext DbContext()
        {
            return this.dbContext;
        }

        protected virtual IQueryable<TEntity> Query()
        {
            return this.DbSet();
        }
    }
}