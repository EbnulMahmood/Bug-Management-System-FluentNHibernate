using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BaseService
{
    public interface IBaseService<T> where T : class
    {
        Task<IEnumerable<T>> ListEntitiesOrderDescExcludeSoftDelete();
        Task<T> LoadEntity(Guid id);
        Task<bool> CreateEntity(T entity);
        Task<bool> UpdateEntity(T entity);
    }
}
