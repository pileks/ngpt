using Ngpt.Platform.Data.Entities.Users;

namespace Ngpt.Platform.Identity.Data.Interfaces.Providers
{
    public interface ILoggedInUserProvider
    {
        User User { get; }

        bool IsUserLoggedIn { get; }

        void AssignUser(User user);
    }
}
