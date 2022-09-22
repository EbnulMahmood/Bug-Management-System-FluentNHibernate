using Microsoft.AspNetCore.Mvc.ModelBinding;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using System.Web.Mvc;
using DAOs.QADao;
using Services.BaseService;

namespace Services.QAService
{
    public class QAService : BaseService<QA>, IQAService
    {
        private readonly IQADao _qADao;
        public QAService(IQADao qADao) : base(qADao)
        {
            _qADao = qADao;
        }

        public Task<IEnumerable<QA>> ListEntities()
        {
            try
            {
                var entities = _qADao.ListEntitiesOrderDescExcludeSoftDelete();
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
                if (!await _qADao.SoftDeleteEntity(id, deleteStatusCode)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
