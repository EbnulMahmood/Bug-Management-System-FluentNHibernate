using Microsoft.AspNetCore.Mvc.ModelBinding;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using System.Web.Mvc;
using DAOs.DeveloperDao;
using Services.BaseService;

namespace Services.DeveloperService
{
    public class DeveloperService : BaseService<Developer>, IDeveloperService
    {
        private readonly IDeveloperDao _developerDao;
        public DeveloperService(IDeveloperDao developerDao) : base(developerDao)
        {
            _developerDao = developerDao;
        }

        public Task<IEnumerable<Developer>> ListEntities()
        {
            try
            {
                var entities = _developerDao.ListEntitiesOrderDescExcludeSoftDelete();
                return entities;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> SoftDeleteEntity(Guid id, int deleteStatusCode)
        {
            try
            {
                if (id == Guid.Empty) return false; 
                var entiry = await _developerDao.LoadEntity(id);
                if (entiry == null) return false;
                entiry.Status = deleteStatusCode;
                if (!await _developerDao.UpdateEntity(entiry)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
