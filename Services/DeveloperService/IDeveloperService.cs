using Entities;
using Services.BaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.DeveloperService
{
    public interface IDeveloperService : IBaseService<Developer>
    {
        Task<IEnumerable<Developer>> ListEntities();
        Task<bool> SoftDeleteEntity(Guid id, int deleteStatusCode);
    }
}
