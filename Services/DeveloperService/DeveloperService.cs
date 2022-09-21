﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            var entities = await _developerDao.ListDevelopersDescExclude404();

            return entities;
        }
        
        public async Task<bool> CreateEntity(Developer entity)
        {
            try
            {
                if(!await _developerDao.CreateDeveloper(entity)) return false;
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
                if (!await _developerDao.UpdateDeveloper(entity)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEntity(Guid? id)
        {
            try
            {
                if (!await _developerDao.DeleteDeveloperInclude404(id)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Developer?> GetEntity(Guid? id)
        {
            try
            {
                if (id == null) return null;

                var entity = await _developerDao.GetDeveloperExclude404(id);
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
