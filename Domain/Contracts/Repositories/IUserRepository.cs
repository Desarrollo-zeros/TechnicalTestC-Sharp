﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts.Repositories
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
    }
}
