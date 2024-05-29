using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreationAuditedEntity
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual string? CreatorUserId { get; set; }

        protected CreationAuditedEntity()
        {
            CreationTime = DateTime.Now;
        }
    }
}
