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
using Entities.BaseEntity;

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

        public async Task<bool> DeleteEntity(Guid id)
        {
            try
            {
                int deleteStatusCode = 404; // status for delete 404
                if (!await _developerDao.SoftDeleteEntity(id, deleteStatusCode)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
