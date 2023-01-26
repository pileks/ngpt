using System;
using System.Threading.Tasks;

namespace Augur.Data.Services
{
    public interface ITransactionManager
    {
        Task UseTransactionAsync(Func<Task> action);
        Task<T> UseTransactionAsync<T>(Func<Task<T>> action);
        Task UseTransactionAsync(Action action);
        Task<T> UseTransactionAsync<T>(Func<T> action);
    }
}