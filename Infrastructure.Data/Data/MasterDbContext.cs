using Domain.Contracts.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Data
{
    public class MasterDbContext(DbContextOptions<MasterDbContext> options) : DbContext(options), IMasterDbContext
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }


        public async Task<ITransaction> BeginTransactionAsync()
        {
            var transaction = await Database.BeginTransactionAsync();
            return new EfCoreTransaction(transaction);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Users)
                .WithOne(u => u.Organization)
                .HasForeignKey(u => u.OrganizationId);
        }
    }
}
