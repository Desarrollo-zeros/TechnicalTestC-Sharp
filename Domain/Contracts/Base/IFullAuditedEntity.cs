using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Base
{
    public interface IFullAuditedEntity
    {
        public bool IsDeleted { get; set; }

        public string? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
