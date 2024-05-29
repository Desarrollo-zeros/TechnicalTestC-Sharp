using Domain.Base;
using Domain.Contracts.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product(string name, string description, Guid organizationId) : FullAuditedEntity<Guid>, ITenant
    {
        [Required]
        public string Name { get; set; } = name;
        [Required]
        public string Description { get; set; } = description;
        [Required]
        public Guid OrganizationId { get; set; } = organizationId;

        public Organization Organization { get; set; }
        public Guid? TenantId { get; set; } = Guid.Empty;
    }
}
