using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;
using NHibernate;
using NHibernate.Criterion;

namespace DAOs.QADao
{
    public class QADao : IQADao
    {
        private readonly ISession _session;
        private readonly ICriteria _qACriteria;

        public QADao(ISession session)
        {
            _session = session;
            _qACriteria = _session.CreateCriteria<QA>();
        }

        public async Task<IEnumerable<QA>> ListQAsDescExclude404()
        {
            using var transaction = _session.BeginTransaction();
            int deleteStatus = 404;
            try
            {
                IEnumerable<QA> entities = await _qACriteria
                    .AddOrder(Order.Desc("CreatedAt"))
                    .Add(!Restrictions.Eq("Status", deleteStatus))
                    .ListAsync<QA>();
                await transaction.CommitAsync();

                return entities;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception();
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
        }

        public async Task<bool> CreateQA(QA qAToCreate)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                await _session.SaveAsync(qAToCreate);
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception();
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
        }

        public async Task<bool> UpdateQA(QA qAToUpdate)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                await _session.UpdateAsync(qAToUpdate);
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception();
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
        }
        
        public async Task<bool> DeleteQAInclude404(Guid? qAToGetId)
        {
            using var transaction = _session.BeginTransaction();
            int deleteStatus = 404;
            try
            {
                // var entity = await _session.GetAsync<QA>(qAToGetId);
                var devList = await _qACriteria.Add(Restrictions.Eq("Id", qAToGetId))
                    .SetMaxResults(1)
                    .ListAsync<QA>();

                var entity = devList.First(); // get first entity from list

                if (entity == null || entity.Status == deleteStatus) return false;
                entity.Status = deleteStatus;
                await _session.UpdateAsync(entity);
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception();
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
        }

        public async Task<QA?> GetQAExclude404(Guid? qAToGetId)
        {
            using var transaction = _session.BeginTransaction();
            int deleteStatus = 404;
            try
            {
                // var entity = await _session.GetAsync<QA>(qAToGetId);
                var devList = await _qACriteria.Add(Restrictions.Eq("Id", qAToGetId))
                    .SetMaxResults(1)
                    .ListAsync<QA>();

                var entity = devList.First(); // get first entity from list

                if (entity == null || entity.Status == deleteStatus) return null;
                await transaction.CommitAsync();

                return entity;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception();
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
        }
    }
}
