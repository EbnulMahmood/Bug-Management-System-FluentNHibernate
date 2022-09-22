using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs.BaseDao
{
    public interface IBaseDao<T> where T : class
    {
        Task<T> LoadEntity(Guid id);
        Task<bool> CreateEntity(T entity);
        Task<bool> UpdateEntity(T entity);
    }
}
