using System;
using System.Linq.Expressions;

namespace Augur.Web.Controllers.GridModels
{
    public class GridColumnDefinition<TModel>
    {
        public string Title { get; set; }
        public Expression<Func<TModel, object>> Property { get; set; }
    }
}
