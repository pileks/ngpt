using System.Collections.Generic;

namespace Augur.Web.Controllers.GridModels
{
    public class GridMetadata
    {
        public IEnumerable<GridColumnMetadata> Columns { get; set; }
        public IEnumerable<GridFilterMetadata> Filters { get; set; }
    }
}