namespace Augur.Data.Interfaces.Providers
{
    public interface IAugurLoggedInTenantIdProvider
    {
        int TenantId { get; }

        void RequireTenantLoggedIn();
    }
}