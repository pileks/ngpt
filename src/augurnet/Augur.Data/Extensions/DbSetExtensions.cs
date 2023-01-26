using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Augur.Entity.Interfaces.Base;
using Augur.Data.Extensions;

namespace Augur.Data.Extensions
{
    public static class DbSetExtensions
    {
        public static IQueryable<TEntity> QueryByIds<TEntity, TId>(this DbSet<TEntity> dbSet, IEnumerable<TId> ids)
            where TEntity : class, IAugurEntityWithId
        {
            var idsList = ids.ToList();

            return dbSet.QueryByIds(idsList);
        }

        public static IQueryable<TEntity> QueryByIds<TEntity>(this DbSet<TEntity> dbSet, IList<int> ids)
            where TEntity : class, IAugurEntityWithId
        {
            return dbSet.Where(e => ids.Contains(e.Id));
        }

        public static async Task<IList<TEntity>> ListAsync<TEntity>(this DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> whereExpr = null)
            where TEntity : class, IAugurEntityWithId
        {
            if (whereExpr != null)
            {
                return await dbSet
                    .Where(whereExpr)
                    .ToListAsync();
            }
            else
            {
                return await dbSet
                    .ToListAsync();
            }
        }

        public static async Task<bool> AnyAsync<TEntity, TId>(this DbSet<TEntity> dbSet, TId id)
            where TEntity : class
        {
            return await dbSet.FindAsync(id) != null;
        }

        public static async Task RemoveAsync<TEntity>(this DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> whereExpr = null)
            where TEntity : class
        {
            if (whereExpr != null)
            {
                dbSet.RemoveRange
                (
                    await dbSet.Where(whereExpr).ToListAsync()
                );
            }
            else
            {
                dbSet.RemoveRange
                (
                    await dbSet.ToListAsync()
                );
            }
        }

        public static async Task RemoveAllAsync<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            await dbSet.RemoveAsync();
        }
    }
}