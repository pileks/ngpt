using System.Collections.Generic;

namespace Augur.Web.Controllers.GridModels
{
    public class GridResult<TModel>
    {
        public ICollection<TModel> Data { get; set; }
        public GridMetadata Metadata { get; set; }
        public int Count { get; internal set; }
    }
}