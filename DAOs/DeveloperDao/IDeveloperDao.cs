using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs.DeveloperDao
{
    public interface IDeveloperDao
    {
        Task<IEnumerable<Developer>> ListDevelopersDescExclude404();
        Task<bool> CreateDeveloper(Developer developerToCreate);
        Task<bool> UpdateDeveloper(Developer developerToUpdate);
        Task<Developer?> GetDeveloperExclude404(Guid? developerToGetId);
        Task<bool> DeleteDeveloperInclude404(Guid? developerToGetId);
    }
}
