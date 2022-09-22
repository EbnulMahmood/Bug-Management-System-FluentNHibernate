using DAOs.BaseDao;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BaseService
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseDao<T> _baseDao;

        public BaseService(IBaseDao<T> baseDao)
        {
            _baseDao = baseDao;
        }

        public async Task<T> LoadEntity(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return null;
                var entity = await _baseDao.LoadEntity(id);
                if (entity == null) return null;
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateEntity(T entity)
        {
            try
            {
                if (!await _baseDao.CreateEntity(entity)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateEntity(T entity)
        {
            try
            {
                if (!await _baseDao.UpdateEntity(entity)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
