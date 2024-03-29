﻿using DAOs.BaseDao;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs.DeveloperDao
{ 
    public interface IDeveloperDao : IBaseDao<Developer>
    {
        Task<IEnumerable<Developer>> ListEntitiesOrderDescExcludeSoftDelete();
        Task<bool> SoftDeleteEntity(Guid id, int deleteStatusCode);
    }
}
