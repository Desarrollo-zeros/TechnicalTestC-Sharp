using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Data
{
    public class StateHelper
    {
        public static EntityState ConvertState(EntityState state)
        {
            return state switch
            {
                EntityState.Detached => EntityState.Unchanged,
                EntityState.Unchanged => EntityState.Unchanged,
                EntityState.Added => EntityState.Added,
                EntityState.Deleted => EntityState.Deleted,
                EntityState.Modified => EntityState.Modified,
                _ => throw new ArgumentOutOfRangeException("state"),
            };
        }
    }
}
