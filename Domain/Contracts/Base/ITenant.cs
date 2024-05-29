using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Base
{
    public interface ITenant
    {
        public Guid? TenantId { get; set; }
    }
}
