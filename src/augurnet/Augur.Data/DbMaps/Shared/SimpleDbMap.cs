using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Augur.Data.DbMaps.Shared
{
    public class SimpleDbMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        private readonly string tableName;

        public SimpleDbMap(string tableName)
        {
            this.tableName = tableName;
        }

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(this.tableName);
        }
    }
}