using System;
using System.Linq.Expressions;

namespace Augur.Web.Controllers.GridModels
{
    public class GridFilterDefinition<TEntity>
    {
        public string Title { get; set; }
        public Expression<Func<TEntity, object>> Property { get; set; }
    }
}
