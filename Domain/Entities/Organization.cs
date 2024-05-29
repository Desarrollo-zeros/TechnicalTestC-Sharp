using Domain.Base;
using Domain.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Organization : FullAuditedEntity<Guid>, ITenant
    {
        public string Name { get; set; } = string.Empty;
        public string SlugTenant { get; set; } = string.Empty;
        public Guid? TenantId { get; set; } = Guid.Empty;

        public ICollection<Product> Products { get; set; }
        public ICollection<User> Users { get; set; }

        public Organization()
        {
            Products = [];
            Users = [];
        }
    }
}
