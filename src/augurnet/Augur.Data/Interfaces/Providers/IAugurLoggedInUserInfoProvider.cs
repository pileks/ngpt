namespace Augur.Data.Interfaces.Providers
{
    public interface IAugurLoggedInUserInfoProvider
    {
        bool IsSuperAdmin { get; }
        bool IsLoggedIn { get; }
    }
}