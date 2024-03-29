﻿using Entities;
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
