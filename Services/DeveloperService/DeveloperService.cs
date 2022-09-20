using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sessions;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using FluentNHibernate.Data;
using System.Web.Mvc;
using DAOs.DeveloperDao;

namespace Services.DeveloperService
{
    public class DeveloperService : IDeveloperService
    {
        private readonly IDeveloperDao _developerDao;
        public DeveloperService()
        {
            _developerDao = new DeveloperDao();
        }

        public async Task<IEnumerable<Developer>> ListDevelopersDescExclude404()
        {
            var entities = await _developerDao.ListDevelopersDescExclude404();

            return entities;
        }
        
        public async Task<bool> CreateDeveloper(Developer developerToCreate)
        {
            try
            {
                if(!await _developerDao.CreateDeveloper(developerToCreate)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateDeveloper(Developer developerToUpdate)
        {
            try
            {
                if (!await _developerDao.UpdateDeveloper(developerToUpdate)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteDeveloperInclude404(Guid? developerToGetId)
        {
            try
            {
                if (!await _developerDao.DeleteDeveloperInclude404(developerToGetId)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Developer?> GetDeveloperExclude404(Guid? developerToGetId)
        {
            try
            {
                if (developerToGetId == null) return null;

                var entity = await _developerDao.GetDeveloperExclude404(developerToGetId);
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
