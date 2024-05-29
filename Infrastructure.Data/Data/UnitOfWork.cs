using Domain.Contracts.Data;
using Domain.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Data
{
    public class UnitOfWork<TDbContext, IRepository, TEntity, TKey> : IUnitOfWork<TDbContext, IRepository, TEntity, TKey>, IDisposable
        where TEntity : class
        where TDbContext : IApplicationDbContext
        where IRepository : IGenericRepository<TEntity, TKey>
    {
        private readonly DbContext _context;
        private readonly IRepository _repository;
        private IDbContextTransaction _transaction;
        private bool _disposed;

        public UnitOfWork(TDbContext context, IRepository repository)
        {
            _context = context as DbContext ?? throw new ArgumentNullException(nameof(context));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IRepository Repository => _repository;

        /// <summary>
        /// Saves the changes asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of state entries written to the database.</returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Begins a new transaction asynchronously.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task BeginTransactionAsync(string savepoint= "default")
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }
            _transaction = await _context.Database.BeginTransactionAsync();
            _transaction.CreateSavepoint(savepoint);
        }

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }

            try
            {
                await _transaction.CommitAsync();
                await _context.Database.CommitTransactionAsync();
                _context.ChangeTracker.Clear();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task RollbackTransactionAsync(string savepoint = "default")
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }
            await _transaction.RollbackAsync();
            await _transaction.RollbackToSavepointAsync(savepoint);
            await _transaction.DisposeAsync();
            await _context.Database.RollbackTransactionAsync();
            _transaction = null;
            _context.ChangeTracker.Clear();
        }

        /// <summary>
        /// Disposes the context and transaction.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the context and transaction.
        /// </summary>
        /// <param name="disposing">Indicates whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

    }
}
