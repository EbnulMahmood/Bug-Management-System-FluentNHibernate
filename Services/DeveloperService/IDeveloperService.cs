using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.DeveloperService
{
    public interface IDeveloperService
    {
        Task<IEnumerable<Developer>> ListEntities();
        Task<bool> CreateEntity(Developer entity);
        Task<bool> UpdateEntity(Developer entity);
        Task<Developer?> GetEntity(Guid id);
        Task<bool> SoftDeleteEntity(Guid id, int deleteStatusCode);
    }
}
