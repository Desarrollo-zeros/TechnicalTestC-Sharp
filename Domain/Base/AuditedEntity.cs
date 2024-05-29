using Domain.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public abstract class AuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>, IAuditedEntity
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual string? LastModifierUserId { get; set; }
    }
}
