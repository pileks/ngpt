using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Augur.Data.DbMaps.Shared
{
    public class AugurEntityDbMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        private readonly LambdaExpression[] queryFilters;

        public AugurEntityDbMap(params LambdaExpression[] queryFilters)
        {
            this.queryFilters = queryFilters;
        }

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (this.queryFilters != null)
            {
                foreach (var queryFilter in this.queryFilters)
                {
                    builder.HasQueryFilter(queryFilter);
                }
            }
        }
    }
}