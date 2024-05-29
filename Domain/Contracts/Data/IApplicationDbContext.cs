using Domain.Contracts.Data;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Data
{
    public interface IApplicationDbContext : IDisposable
    {
       
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<ITransaction> BeginTransactionAsync();


    }
}
