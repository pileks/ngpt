using Augur.Data.DbContextAddons;
using Augur.Data.Interfaces.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Augur.Repositories.Extensions
{
    public class MultitenancyConfigurationBuilder
    {
        private readonly IServiceCollection services;

        public MultitenancyConfigurationBuilder(IServiceCollection services)
        {
            services.AddTransient<AugurMultiTenancyDbContextAddon, AugurMultiTenancyDbContextAddon>();

            this.services = services;
        }

        public MultitenancyConfigurationBuilder ConfigureLoggedInTenantIdProvider<TILoggedInTenantIdProvider,
            TLoggedInTenantIdProvider>()
            where TLoggedInTenantIdProvider : class, TILoggedInTenantIdProvider
            where TILoggedInTenantIdProvider : class, IAugurLoggedInTenantIdProvider
        {
            this.services
                .RegisterTransient<IAugurLoggedInTenantIdProvider, TILoggedInTenantIdProvider, TLoggedInTenantIdProvider
                >();

            return this;
        }
    }
}