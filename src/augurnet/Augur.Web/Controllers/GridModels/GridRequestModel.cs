using System.Collections.Generic;

namespace Augur.Web.Controllers.GridModels
{
    public class GridRequestModel
    {
        public GridRequestModel()
        {
            SortBy = new List<GridSortModel>();
            Filters = new List<GridFilterModel>();
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public IEnumerable<GridSortModel> SortBy { get; set; }
        public IEnumerable<GridFilterModel> Filters { get; set; }
    }

    public class GridSortModel
    {
        public string Column { get; set; }
        public string Direction { get; set; }
    }

    public class GridFilterModel
    {
        public string Property { get; set; }
        public GridFilterOperator Operator { get; set; }
        public object Value { get; set; }
    }
}