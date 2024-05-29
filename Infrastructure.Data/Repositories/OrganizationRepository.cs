using Domain.Entities;
using Domain.Contracts.Repositories;
using Domain.Contracts.Base;
using Infrastructure.Data.Data;
using Domain.Contracts.Data;

namespace Infrastructure.Data.Repositories
{
    public class OrganizationRepository(MasterDbContext context, ITenant tenant) :
        GenericRepository<Organization, Guid, MasterDbContext>(context, tenant),
        IOrganizationRepository
    {
    }
}
