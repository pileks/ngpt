namespace Augur.Data.Interfaces.Providers
{
    public interface IAugurLoggedInUserIdProvider
    {
        int UserId { get; }
        bool IsUserLoggedIn { get; }

        void RequireUserLoggedIn();
    }
}