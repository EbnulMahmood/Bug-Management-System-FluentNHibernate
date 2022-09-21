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

namespace Services.QAService
{
    public class QAService : IQAService
    {
        private readonly IQADao _qADao;
        public QAService(IQADao qADao)
        {
            _qADao = qADao;
        }

        public async Task<IEnumerable<QA>> ListEntities()
        {
            var entities = await _qADao.ListQAsDescExclude404();

            return entities;
        }
        
        public async Task<bool> CreateEntity(QA entity)
        {
            try
            {
                if(!await _qADao.CreateQA(entity)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateEntity(QA entity)
        {
            try
            {
                if (!await _qADao.UpdateQA(entity)) return false;
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
                if (!await _qADao.DeleteQAInclude404(id)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<QA?> GetEntity(Guid? id)
        {
            try
            {
                if (id == null) return null;

                var entity = await _qADao.GetQAExclude404(id);
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
