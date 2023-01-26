using Augur.Entity.Interfaces.Base;

namespace Augur.Entity.Interfaces.Entities
{
    public interface IAugurPermission : ITenantEntity
    {
        string Component { get; set; }
        string Activity { get; set; }
        bool IsAllowed { get; set; }

        IAugurRole Role { get; set; }
    }
}