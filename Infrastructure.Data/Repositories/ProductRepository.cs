using Domain.Entities;
using Domain.Contracts.Repositories;
using Domain.Contracts.Base;
using Infrastructure.Data.Data;
using Domain.Contracts.Data;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository(ITenantDbContext context, ITenant tenant) : 
        GenericRepository<Product, Guid, ITenantDbContext>(context, tenant),
        IProductRepository
    {
    }
}
