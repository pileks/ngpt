using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Augur.Web.Controllers.GridModels
{
    public class GridDefinition<TEntity, TModel>
    {
        public List<GridFilterDefinition<TEntity>> Filters { get; set; }
        public List<GridColumnDefinition<TModel>> Columns { get; set; }
        public Expression<Func<TEntity, TModel>> EntityModelMapperFn { get; set; }
    }
}
