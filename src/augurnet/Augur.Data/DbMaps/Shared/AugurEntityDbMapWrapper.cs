using Augur.Entity.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace Augur.Data.DbMaps.Shared
{
    public class AugurEntityDbMapWrapper<TEntity> : AugurEntityDbMap<TEntity> where TEntity : class
    {
        private readonly IEntityTypeConfiguration<TEntity> entityTypeConfiguration;

        public AugurEntityDbMapWrapper(IEntityTypeConfiguration<TEntity> entityTypeConfiguration,
            params LambdaExpression[] queryFilters)
            : base(queryFilters)
        {
            this.entityTypeConfiguration = entityTypeConfiguration;
        }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            this.entityTypeConfiguration.Configure(builder);

            base.Configure(builder);
        }
    }
}