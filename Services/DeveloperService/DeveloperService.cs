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

namespace Services.DeveloperService
{
    public class DeveloperService : IDeveloperService
    {
        private readonly IDeveloperDao _developerDao;
        public DeveloperService(IDeveloperDao developerDao)
        {
            _developerDao = developerDao;
        }

        public async Task<IEnumerable<Developer>> ListEntities()
        {
            IEnumerable<Developer> developersList = await _developerDao.ListEntities();

            int deleteStatusCode = 404; // status delete code 404
            var entities = developersList.OrderByDescending(d => d.CreatedAt)
                .Where(d => d.Status != deleteStatusCode).ToList();

            return entities;
        }
        
        public async Task<bool> CreateEntity(Developer entity)
        {
            try
            {
                if(!await _developerDao.CreateEntity(entity)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateEntity(Developer entity)
        {
            try
            {
                if (!await _developerDao.UpdateEntity(entity)) return false;
                return true;
            }
            catch
            {
                return false;
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

        public async Task<Developer?> GetEntity(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return null;
                var entity = await _developerDao.LoadEntity(id);
                if (entity == null) return null;
                return entity;
            }
            catch
            {
                return null;
            }
        }
    }
}
