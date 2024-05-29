using Domain.Contracts.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Data
{
    public class TenantDbContext(DbContextOptions<TenantDbContext> options) : DbContext(options), ITenantDbContext
    {
        public DbSet<Product> Products { get; set; }

        public async Task<ITransaction> BeginTransactionAsync()
        {
            var transaction = await Database.BeginTransactionAsync();
            return new EfCoreTransaction(transaction);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
