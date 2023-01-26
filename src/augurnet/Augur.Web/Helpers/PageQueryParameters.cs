using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Augur.Web.Helpers
{
    public class PagingQueryParameters
    {
        const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 25;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string SearchQuery { get; set; }
    }
}