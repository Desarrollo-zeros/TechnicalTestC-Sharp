using Domain.Contracts.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Data
{
    public class EfCoreTransaction(IDbContextTransaction dbContextTransaction) : ITransaction
    {
        private readonly IDbContextTransaction _dbContextTransaction = dbContextTransaction;

        public async Task CommitAsync()
        {
            await _dbContextTransaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _dbContextTransaction.RollbackAsync();
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }
    }
}
