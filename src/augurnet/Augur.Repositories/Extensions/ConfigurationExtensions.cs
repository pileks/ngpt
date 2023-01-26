using System;
using Augur.Data.DbContextAddons;
using Microsoft.Extensions.DependencyInjection;

namespace Augur.Repositories.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureChangeTracking(this IServiceCollection services)
        {
            services.AddTransient<AugurChangeTrackingDbContextAddon>();
        }

        public static MultitenancyConfigurationBuilder ConfigureMultitenancy(this IServiceCollection services)
        {
            return new MultitenancyConfigurationBuilder(services);
        }

        public static IServiceCollection RegisterTransient<TIVendorDependency, TIDependency, TDependency>(
            this IServiceCollection services)
            where TDependency : class, TIDependency
            where TIDependency : class, TIVendorDependency
            where TIVendorDependency : class
        {
            services.AddTransient<TIVendorDependency, TDependency>();
            services.AddTransient<TIDependency, TDependency>();
            services.AddTransient<TDependency>();

            return services;
        }

        public static IServiceCollection RegisterTransient(this IServiceCollection services, Type vendorDependencyType,
            Type dependencyInterfaceType, Type dependencyType)
        {
            services.AddTransient(vendorDependencyType, dependencyType);
            services.AddTransient(dependencyInterfaceType, dependencyType);
            services.AddTransient(dependencyType);

            return services;
        }

        public static IServiceCollection RegisterScoped<TIVendorDependency, TIDependency, TDependency>(
            this IServiceCollection services)
            where TIDependency : class, TIVendorDependency
            where TIVendorDependency : class
            where TDependency : class, TIDependency
        {
            services.AddScoped<TDependency>();
            services.AddScoped<TIDependency>(x => x.GetRequiredService<TDependency>());
            services.AddScoped<TIVendorDependency>(x => x.GetRequiredService<TDependency>());

            return services;
        }

        public static IServiceCollection RegisterScoped<TIDependency, TDependency>(this IServiceCollection services)
            where TIDependency : class
            where TDependency : class, TIDependency
        {
            services.AddScoped<TDependency>();
            services.AddScoped<TIDependency>(x => x.GetRequiredService<TDependency>());

            return services;
        }
    }
}