using Augur.Data.DbMaps.Shared;
using Augur.Entity.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace Augur.Data.Extensions
{
    public static class DbMapExtensions
    {
        public static void BuildDefaultPostgresTableColumnsConfiguration<TEntity>(
            this EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            var entityType = typeof(TEntity);

            foreach (var property in entityType.GetProperties())
            {
                if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                {
                    builder.Property(property.Name).HasColumnName(property.Name.ToSnakeCase());
                }
            }
        }

        public static IEntityTypeConfiguration<TEntity> WithQueryFilters<TEntity>(
            this IEntityTypeConfiguration<TEntity> entityTypeConfiguration,
            params Expression<Func<TEntity, bool>>[] queryFilters)
            where TEntity : class
        {
            return new AugurEntityDbMapWrapper<TEntity>(entityTypeConfiguration, queryFilters);
        }

        public static IEntityTypeConfiguration<TEntity> WithQueryFilters<TEntity>(
            this IEntityTypeConfiguration<TEntity> entityTypeConfiguration,
            params LambdaExpression[] queryFilters)
            where TEntity : class
        {
            return new AugurEntityDbMapWrapper<TEntity>(entityTypeConfiguration, queryFilters);
        }
    }
}