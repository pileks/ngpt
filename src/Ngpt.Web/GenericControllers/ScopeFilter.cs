using Augur.Web.Helpers;

namespace Ngpt.Web.GenericControllers
{
    public class ScopeFilter : IColumnFilter
    {
        public bool ShouldFilterForOrganization { get; set; }
        public bool ShouldFilterOwn { get; set; }
    }
}