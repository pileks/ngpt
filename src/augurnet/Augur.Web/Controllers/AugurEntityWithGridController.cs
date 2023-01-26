using Augur.Entity.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System;
using Augur.Web.Controllers.GridModels;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Augur.Entity.Base.Entities;
using System.Linq.Expressions;
using Augur.BackendToFrontendExporter.Extensions;
using System.Linq.Dynamic.Core;
using Augur.Web.Helpers;

namespace Augur.Web.Controllers
{
    public abstract class AugurEntityWithGridController<TEntity, TGridModel> : AugurEntityWithGridController<TEntity, TGridModel, IColumnFilter>
        where TEntity : class, IAugurEntityWithId
        where TGridModel : class
    {
        public AugurEntityWithGridController(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public abstract class AugurEntityWithGridController<TEntity, TGridModel, TColumnFilter> : AugurEntityController<TEntity, int, TColumnFilter>
        where TEntity : class, IAugurEntityWithId
        where TColumnFilter : class, IColumnFilter
        where TGridModel : class
    {
        public AugurEntityWithGridController(DbContext dbContext) : base(dbContext)
        {
        }

        protected abstract GridDefinition<TEntity, TGridModel> GetGridDefinition();

        [HttpPost(nameof(Grid))]
        public virtual async Task<ActionResult<GridResult<TGridModel>>> Grid([FromBody] GridRequestModel requestModel)
        {
            return await GetGrid(requestModel, this.GetGridDefinition(), Query());
        }

        protected virtual async Task<ActionResult<GridResult<TGridModel>>> GetGrid(GridRequestModel requestModel, GridDefinition<TEntity, TGridModel> gridDefinition, IQueryable<TEntity> query)
        {
            query = ApplyGridFilters(query, requestModel);
            query = ApplySearchQueryFilter(query, requestModel.Search);

            if (!requestModel.SortBy.Any())
            {
                query = ApplyOrdering(query);
            }

            var entries = query.Count();

            var gridData = query.Select(gridDefinition.EntityModelMapperFn);
            gridData = ApplyGridSorts(gridData, requestModel);
            gridData = ApplyGridPaging(gridData, requestModel);

            var data = await gridData.ToListAsync();

            var metadata = GetGridMetadata(gridDefinition);

            return Ok(new GridResult<TGridModel>
            {
                Data = data,
                Count = entries,
                Metadata = metadata
            });
        }

        private GridMetadata GetGridMetadata(GridDefinition<TEntity, TGridModel> gridDefinition)
        {
            var columnMeta = gridDefinition.Columns.Select(GetGridColumnMetadata);

            var filterMeta = gridDefinition.Filters != null
                ? gridDefinition.Filters.Select(GetGridFilterMetadata)
                : new GridFilterMetadata[] { };

            return new GridMetadata
            {
                Columns = columnMeta,
                Filters = filterMeta
            };
        }

        private static GridColumnMetadata GetGridColumnMetadata(GridColumnDefinition<TGridModel> x)
        {
            var member = GetMemberExpression(x.Property);

            var propertyTypeDescription = new Augur.BackendToFrontendExporter.TypeDescription(member.Type);

            return new GridColumnMetadata
            {
                Title = x.Title,
                Property = GetGridColumnPropertyName(x.Property),
                Type = propertyTypeDescription.TypescriptType
            };
        }

        private static GridFilterMetadata GetGridFilterMetadata(GridFilterDefinition<TEntity> filterDefinition)
        {
            var member = GetMemberExpression(filterDefinition.Property);

            var propertyTypeDescription = new Augur.BackendToFrontendExporter.TypeDescription(member.Type);

            var propertyName = member.ToString();
            var isAugurEntity = member.Type.IsSubclassOf(typeof(AugurEntityWithId));

            if (isAugurEntity)
            {
                propertyName += ".Id";
            }

            var operators = new GridFilterOperatorModel[] { };

            if (isAugurEntity)
            {
                operators = entityGridFilterOperators;
            }
            else if (propertyTypeDescription.Type.IsEnum)
            {
                operators = enumGridFilterOperators;
            }
            else
            {
                switch (propertyTypeDescription.TypescriptType)
                {
                    case "string":
                        operators = stringGridFilterOperators;
                        break;
                    case "number":
                        operators = numberGridFilterOperators;
                        break;
                    case "Date":
                        operators = dateGridFilterOperators;
                        break;
                    case "boolean":
                        operators = booleanGridFilterOperators;
                        break;
                }
            }

            return new GridFilterMetadata
            {
                Title = filterDefinition.Title,
                Property = TruncateMemberExpressionParameter(propertyName),
                Type = propertyTypeDescription.TypescriptType,
                IsAugurEntity = isAugurEntity,
                IsEnum = propertyTypeDescription.Type.IsEnum,
                Operators = operators
            };
        }

        private static IQueryable<TEntity> ApplyGridFilters(IQueryable<TEntity> query, GridRequestModel requestModel)
        {
            foreach (var filter in requestModel.Filters)
            {
                switch (filter.Operator)
                {
                    case GridFilterOperator.Equals:
                        query = query.Where(filter.Property + " == @0", filter.Value);
                        break;
                    case GridFilterOperator.Greater:
                        query = query.Where(filter.Property + " > @0", filter.Value);
                        break;
                    case GridFilterOperator.Less:
                        query = query.Where(filter.Property + " < @0", filter.Value);
                        break;
                    case GridFilterOperator.GreaterOrEqual:
                        query = query.Where(filter.Property + " >= @0", filter.Value);
                        break;
                    case GridFilterOperator.LessOrEqual:
                        query = query.Where(filter.Property + " <= @0", filter.Value);
                        break;
                    case GridFilterOperator.Contains:
                        query = query.Where(filter.Property + ".Contains(@0)", filter.Value);
                        break;
                    default:
                        throw new NotSupportedException("Unsupported filter operator!");
                }
            }

            return query;
        }

        private static IQueryable<TGridModel> ApplyGridPaging(IQueryable<TGridModel> query, GridRequestModel requestModel)
        {
            query = query
                .Skip((requestModel.Page - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize);
            return query;
        }

        private static IQueryable<TGridModel> ApplyGridSorts(IQueryable<TGridModel> query, GridRequestModel requestModel)
        {
            if (requestModel.SortBy.Count() == 0)
            {
                return query;
            }

            IOrderedQueryable<TGridModel> orderedQuery;

            var parsingConfig = new ParsingConfig { IsCaseSensitive = false };

            var firstSortModel = requestModel.SortBy.First();

            orderedQuery = firstSortModel.Direction.ToLowerInvariant() switch
            {
                "asc" => query.OrderBy(parsingConfig, firstSortModel.Column),
                "desc" => query.OrderBy(parsingConfig, firstSortModel.Column + " desc"),
                _ => throw new InvalidOperationException("Order value needs to be 'asc' or 'desc'"),
            };

            foreach (var sortProp in requestModel.SortBy.Skip(1))
            {
                orderedQuery = sortProp.Direction.ToLowerInvariant() switch
                {
                    "asc" => orderedQuery.ThenBy(parsingConfig, sortProp.Column),
                    "desc" => orderedQuery.ThenBy(parsingConfig, sortProp.Column + " desc"),
                    _ => throw new InvalidOperationException("Order value needs to be 'asc' or 'desc'"),
                };
            }

            return orderedQuery;
        }

        private static string GetGridColumnPropertyName<TModelType>(Expression<Func<TModelType, object>> propertyExpression)
        {
            MemberExpression memeberExpression = GetMemberExpression(propertyExpression);

            var memeberExpressionString = memeberExpression.ToString();

            return TruncateMemberExpressionParameter(memeberExpressionString).ToCamelCase();
        }

        private static string TruncateMemberExpressionParameter(string memeberExpressionString)
        {
            return memeberExpressionString.Substring(memeberExpressionString.IndexOf('.') + 1);
        }

        private static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> propertyExpression)
        {
            var unary = propertyExpression.Body as UnaryExpression;
            var member = propertyExpression.Body as MemberExpression;

            var memeberExpression = member ?? (unary != null ? unary.Operand as MemberExpression : null);
            return memeberExpression;
        }

        private static GridFilterOperatorModel[] stringGridFilterOperators = new GridFilterOperatorModel[]
        {
            new GridFilterOperatorModel
            {
                Title = "contains",
                Operator = GridFilterOperator.Contains
            },
            new GridFilterOperatorModel
            {
                Title = "is exactly",
                Operator = GridFilterOperator.Equals
            }
        };

        private static GridFilterOperatorModel[] numberGridFilterOperators = new GridFilterOperatorModel[]
        {
            new GridFilterOperatorModel
            {
                Title = "is equal to",
                Operator = GridFilterOperator.Equals
            },
            new GridFilterOperatorModel
            {
                Title = "is greater than",
                Operator = GridFilterOperator.Greater
            },
            new GridFilterOperatorModel
            {
                Title = "is less than",
                Operator = GridFilterOperator.Less
            },
            new GridFilterOperatorModel
            {
                Title = "is greater or equal to",
                Operator = GridFilterOperator.GreaterOrEqual
            },
            new GridFilterOperatorModel
            {
                Title = "is less or equal to",
                Operator = GridFilterOperator.LessOrEqual
            }
        };

        private static GridFilterOperatorModel[] dateGridFilterOperators = new GridFilterOperatorModel[]
        {
            new GridFilterOperatorModel
            {
                Title = "is exactly on",
                Operator = GridFilterOperator.Equals
            },
            new GridFilterOperatorModel
            {
                Title = "is after",
                Operator = GridFilterOperator.Greater
            },
            new GridFilterOperatorModel
            {
                Title = "is before",
                Operator = GridFilterOperator.Less
            },
            new GridFilterOperatorModel
            {
                Title = "is after or on",
                Operator = GridFilterOperator.GreaterOrEqual
            },
            new GridFilterOperatorModel
            {
                Title = "is before or on",
                Operator = GridFilterOperator.LessOrEqual
            }
        };

        private static GridFilterOperatorModel[] booleanGridFilterOperators = new GridFilterOperatorModel[]
        {
            new GridFilterOperatorModel
            {
                Title = "is",
                Operator = GridFilterOperator.Equals
            }
        };

        private static GridFilterOperatorModel[] entityGridFilterOperators = new GridFilterOperatorModel[]
        {
            new GridFilterOperatorModel
            {
                Title = "is",
                Operator = GridFilterOperator.Equals
            }
        };

        private static GridFilterOperatorModel[] enumGridFilterOperators = new GridFilterOperatorModel[]
        {
            new GridFilterOperatorModel
            {
                Title = "is",
                Operator = GridFilterOperator.Equals
            }
        };
    }
}
