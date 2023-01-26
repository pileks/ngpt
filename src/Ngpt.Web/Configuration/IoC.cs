using Augur.ApiSystemEvents;
using Augur.Data.DbContextAddons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ngpt.Repositories.Users;
using Augur.Data.Interfaces.Providers;
using Augur.Data.Services;
using Augur.Emails.EmailBuilding;
using Augur.Emails.Settings;
using Microsoft.AspNetCore.Identity;
using Augur.Repositories.Extensions;
using Ngpt.Data.DbContexts;
using Ngpt.Web.ApplicationSettings;
using Microsoft.Data.SqlClient;
using Ngpt.Data;
using Ngpt.Data.Services;
using Ngpt.Platform.FileResources.Web.ApplicationSettings;
using Ngpt.Platform.Identity.Web.Providers;
using Ngpt.Platform.Identity.Web.Services;
using Ngpt.Platform.Identity.Web.Settings;
using Ngpt.Platform.Identity.Data.Providers;
using Ngpt.Platform.Identity.Data.Interfaces.Providers;
using Ngpt.Platform.Multitenancy.Data.Interfaces.Providers;
using Ngpt.Platform.Multitenancy.Data.Providers;
using Ngpt.Platform.Services;

namespace Ngpt.Web.Configuration
{

    public static class IoC
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<AppSettings>(x => x.GetService<IConfiguration>().Get<AppSettings>());
            services.AddTransient<EmailSettings>(x => x.GetService<IConfiguration>().Get<AppSettings>().Email);
            services.AddTransient<EmailVerificationSettings>(x => x.GetService<IConfiguration>().Get<AppSettings>().EmailVerification);
            services.AddTransient<UploadedResourcesSettings>(x => x.GetService<IConfiguration>().Get<AppSettings>().UploadedResources);
            services.AddTransient<AuthenticationSettings>(x => x.GetService<IConfiguration>().Get<AppSettings>().Authentication ?? new AuthenticationSettings());

            services.AddHttpContextAccessor();

            var connectionString = configuration.GetConnectionString("NgptDbConnection");
            services.AddScoped(s => new SqlConnection(connectionString));

            services.AddDbContext<RootDbContext>((s, options) => options.UseSqlServer(s.GetRequiredService<SqlConnection>()));
            services.AddDbContext<OrgAdminDbContext>((s, options) => options.UseSqlServer(s.GetRequiredService<SqlConnection>()));
            services.AddDbContext<SuperAdminDbContext>((s, options) => options.UseSqlServer(s.GetRequiredService<SqlConnection>()));

            services.AddTransient(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

            ConfigureRepositories(services);

            ConfigureIdentity(services, configuration);

            ConfigureMultitenancy(services, configuration);

            ConfigureSecurity(services);

            services.ConfigureChangeTracking();
            
            services.AddTransient(typeof(EmailBuilder),typeof(EmailBuilder));
            services.AddTransient(typeof(EmailSender<>),typeof(EmailSender<>));
            services.AddTransient<IHeaderClientNotifier, AugurHeaderClientNotifier>();
            services.AddTransient<AuthenticationService, AuthenticationService>();
            services.AddTransient<ScopedEntitySecurityService, ScopedEntitySecurityService>();
            services.AddTransient<OrgAdminUserOwnershipDbContextAddon, OrgAdminUserOwnershipDbContextAddon>();
            services.AddTransient<SuperAdminTenantOwnershipDbContextAddon, SuperAdminTenantOwnershipDbContextAddon>();
        }

        private static void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<UsersRepository, UsersRepository>();
        }

        private static void ConfigureIdentity(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserOwnedDbContext>((s, options) => options.UseSqlServer(s.GetRequiredService<SqlConnection>()));

            services.AddTransient<AugurUserOwnershipDbContextAddon>();

            services.RegisterScoped<IAugurLoggedInUserIdProvider, ILoggedInUserIdProvider, LoggedInUserIdProvider>()
                    .RegisterTransient<IAugurLoggedInUserInfoProvider, ILoggedInUserInfoProvider, LoggedInUserInfoProvider>();
            
            services.AddScoped<AuthTokenService>();
            services.AddScoped<LoggedInUserProvider>();
            services.AddScoped<ILoggedInUserProvider>(x => x.GetRequiredService<LoggedInUserProvider>());
        }

        private static void ConfigureMultitenancy(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TenantOwnedDbContext>((s, options) => options.UseSqlServer(s.GetRequiredService<SqlConnection>()));
            services.AddDbContext<TenantUserOwnedDbContext>((s, options) => options.UseSqlServer(s.GetRequiredService<SqlConnection>()));

            services.ConfigureMultitenancy()
                .ConfigureLoggedInTenantIdProvider<ILoggedInTenantIdProvider, LoggedInTenantIdProvider>();

            services.RegisterScoped<ITransactionManager, Data.Services.TransactionManager>();
        }

        private static void ConfigureSecurity(IServiceCollection services)
        {
            services.RegisterTransient<Augur.Security.Interfaces.Services.ILoggedInUserAccessManager, Ngpt.Platform.Identity.Data.Interfaces.Services.ILoggedInUserAccessManager, LoggedInUserAccessManager>();
        }
    }
}