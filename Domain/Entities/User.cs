using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : FullAuditedEntity<Guid>
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public Guid OrganizationId { get; set; } = Guid.Empty;

        public Guid? TenantId { get; set; } = Guid.Empty;
        public Organization? Organization { get; set; }
    }
}
