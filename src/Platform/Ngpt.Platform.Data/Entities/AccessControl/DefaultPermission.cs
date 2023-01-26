using Augur.BackendToFrontendExporter;
using Augur.Entity.Base.Entities;

namespace Ngpt.Platform.Data.Entities.AccessControl
{
    [DisableExportToFrontend]
    public class DefaultPermission : AugurEntityWithId
    {
        public string Component { get; set; }
        public string Activity { get; set; }
        public RoleType RoleType { get; set; }
        public bool IsAllowed { get; set; }
    }
}
