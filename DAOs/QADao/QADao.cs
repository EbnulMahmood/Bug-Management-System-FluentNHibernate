using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using NHibernate;
using NHibernate.Linq;

namespace DAOs.QADao
{
    public class QADao : IQADao
    {
        private readonly ISession _session;
        private ITransaction? _transaction;

        public QADao(ISession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<QA>> ListQAsDescExclude404()
        {
            _transaction = _session.BeginTransaction();
            IEnumerable<QA> entities = await _session
                .Query<QA>()
                .OrderByDescending(d => d.CreatedAt)
                .Where(d => d.Status != 404)
                .ToListAsync();

            _transaction.Commit();
            return entities;
        }

        public async Task<bool> CreateQA(QA qAToCreate)
        {
            _transaction = _session.BeginTransaction();
            await _session.SaveAsync(qAToCreate);
            _transaction.Commit();
            return true;
        }

        public async Task<bool> UpdateQA(QA qAToUpdate)
        {
            _transaction = _session.BeginTransaction();
            await _session.UpdateAsync(qAToUpdate);
            _transaction.Commit();
            return true;
        }
        
        public async Task<bool> DeleteQAInclude404(Guid? qAToGetId)
        {
            _transaction = _session.BeginTransaction();
            var entity = await _session.GetAsync<QA>(qAToGetId);

            if (entity.Status == 404) return false;
            entity.Status = 404;
            await _session.UpdateAsync(entity);

            _transaction.Commit();
            return true;
        }

        public async Task<QA?> GetQAExclude404(Guid? qAToGetId)
        {
            _transaction = _session.BeginTransaction();
            var entity = await _session.GetAsync<QA>(qAToGetId);
            _transaction.Commit();

            if (entity == null || entity.Status == 404) return null;

            return entity;
        }
    }
}
