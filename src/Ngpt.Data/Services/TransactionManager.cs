using System;
using System.Threading.Tasks;
using Augur.Data.Extensions;
using Augur.Data.Services;
using Ngpt.Data.DbContexts;

namespace Ngpt.Data.Services
{
    public class TransactionManager : ITransactionManager
    {
        private readonly RootDbContext rootDbContext;
        private readonly TenantOwnedDbContext tenantOwnedDbContext;
        private readonly TenantUserOwnedDbContext tenantUserOwnedDbContext;

        public TransactionManager(RootDbContext rootDbContext, TenantOwnedDbContext tenantOwnedDbContext, 
            TenantUserOwnedDbContext tenantUserOwnedDbContext)
        {
            this.rootDbContext = rootDbContext;
            this.tenantOwnedDbContext = tenantOwnedDbContext;
            this.tenantUserOwnedDbContext = tenantUserOwnedDbContext;
        }

        public async Task UseTransactionAsync(Func<Task> action)
        {
            await this.rootDbContext.UseTransactionAsync(action, this.tenantOwnedDbContext, this.tenantUserOwnedDbContext);
        }

        public async Task<T> UseTransactionAsync<T>(Func<Task<T>> action)
        {
            return await this.rootDbContext.UseTransactionAsync<T>(action, this.tenantOwnedDbContext, this.tenantUserOwnedDbContext);
        }

        public async Task UseTransactionAsync(Action action)
        {
            await this.rootDbContext.UseTransactionAsync(action, this.tenantOwnedDbContext, this.tenantUserOwnedDbContext);
        }

        public async Task<T> UseTransactionAsync<T>(Func<T> action)
        {
            return await this.rootDbContext.UseTransactionAsync<T>(action, this.tenantOwnedDbContext, this.tenantUserOwnedDbContext);
        }
    }
}
