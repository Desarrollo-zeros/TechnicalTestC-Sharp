using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Data
{
    public interface IUnitOfWork<TDbContext, IRepository, TEntity, TKey> : IDisposable
        where TEntity : class
        where IRepository : IGenericRepository<TEntity, TKey>
        where TDbContext : IApplicationDbContext
    {
        IRepository Repository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task  BeginTransactionAsync(string savepoint = "default");
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync(string savepoint = "default");
    }
}
