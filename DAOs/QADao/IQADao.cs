using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOs.BaseDao;
using Entities;

namespace DAOs.QADao
{
    public interface IQADao : IBaseDao<QA>
    {
        Task<IEnumerable<QA>> ListEntitiesOrderDescExcludeSoftDelete();
        Task<bool> SoftDeleteEntity(Guid id, int deleteStatusCode);
    }
}
