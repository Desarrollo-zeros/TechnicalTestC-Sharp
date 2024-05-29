using Domain.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, ISoftDelete, IFullAuditedEntity
    {
        public virtual bool IsDeleted { get; set; }

        public virtual string? DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }
    }
}
