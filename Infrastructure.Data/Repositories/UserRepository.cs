using Domain.Entities;
using Domain.Contracts.Repositories;
using Domain.Contracts.Base;
using Infrastructure.Data.Data;
using Domain.Contracts.Data;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository(MasterDbContext context, ITenant tenant)
        : GenericRepository<User, Guid, MasterDbContext>(context, tenant),
        IUserRepository
    {
    }
}
