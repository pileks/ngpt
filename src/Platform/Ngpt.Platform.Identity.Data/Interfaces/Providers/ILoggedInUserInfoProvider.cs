using Augur.Data.Interfaces.Providers;

namespace Ngpt.Platform.Identity.Data.Interfaces.Providers
{
    public interface ILoggedInUserInfoProvider : IAugurLoggedInUserInfoProvider
    {
        bool HasVerifiedEmail { get; }
    }
}
