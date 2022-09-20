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
        Task<object> ListDevelopersDescExclude404(int draw, int start, int length, string filter_keywords, int filter_option = 0);
        Task<bool> CreateDeveloper(Developer developerToCreate);
        Task<bool> UpdateDeveloper(Developer developerToUpdate);
        Task<Developer?> GetDeveloperExclude404(Guid? developerToGetId);
        Task<bool> DeleteDeveloperInclude404(Guid? developerToGetId);
    }
}
