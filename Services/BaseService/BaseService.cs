using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BaseService
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        public BaseService()
        {
        }

        public Task<bool> CreateEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ListEntitiesOrderDescExcludeSoftDelete()
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadEntity(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEntity(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
