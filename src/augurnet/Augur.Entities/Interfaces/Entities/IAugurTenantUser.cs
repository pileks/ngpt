namespace Augur.Entity.Interfaces.Entities
{
    public interface IAugurTenantUser : IAugurUser
    {
        int TenantId { get; set; }
    }
}