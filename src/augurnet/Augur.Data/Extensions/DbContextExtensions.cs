using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Augur.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task CommitTogetherAsync(this DbContext dbContext, params DbContext[] dbContexts)
        {
            await using var transaction = dbContext.Database.BeginTransaction();

            dbContext.SaveChanges();

            foreach (var context in dbContexts)
            {
                context.Database.UseTransaction(transaction.GetDbTransaction());

                context.SaveChanges();
            }

            transaction.Commit();
        }

        public static async Task UseTransactionAsync(this DbContext dbContext, Func<Task> action,
            params DbContext[] contexts)
        {
            await using var transaction = dbContext.Database.BeginTransaction();

            foreach (var context in contexts)
            {
                context.Database.UseTransaction(transaction.GetDbTransaction());
            }

            await action();

            transaction.Commit();
        }

        public static async Task<T> UseTransactionAsync<T>(this DbContext dbContext, Func<Task<T>> action,
            params DbContext[] contexts)
        {
            await using var transaction = dbContext.Database.BeginTransaction();

            foreach (var context in contexts)
            {
                context.Database.UseTransaction(transaction.GetDbTransaction());
            }

            var result = await action();

            transaction.Commit();

            return result;
        }

        public static async Task UseTransactionAsync(this DbContext dbContext, Action action,
            params DbContext[] contexts)
        {
            await using var transaction = dbContext.Database.BeginTransaction();

            foreach (var context in contexts)
            {
                context.Database.UseTransaction(transaction.GetDbTransaction());
            }

            action();

            transaction.Commit();
        }

        public static async Task<T> UseTransactionAsync<T>(this DbContext dbContext, Func<T> action,
            params DbContext[] contexts)
        {
            await using var transaction = dbContext.Database.BeginTransaction();

            foreach (var context in contexts)
            {
                context.Database.UseTransaction(transaction.GetDbTransaction());
            }

            var result = action();

            transaction.Commit();

            return result;
        }
    }
}