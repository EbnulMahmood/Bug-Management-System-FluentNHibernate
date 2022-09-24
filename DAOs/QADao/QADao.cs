using DAOs.BaseDao;
using Entities;
using NHibernate;
using NHibernate.Criterion;

namespace DAOs.QADao
{
    public class QADao : BaseDao<QA>, IQADao
    {
        private readonly ISession _session;
        private readonly ICriteria _criteria;

        public QADao(ISession session) : base(session)
        {
            _session = session;
            _criteria = _session.CreateCriteria<QA>();
        }

        public async Task<IEnumerable<QA>> ListEntitiesOrderDescExcludeSoftDelete()
        {
            using var transaction = _session.BeginTransaction();
            int deleteStatus = 404;
            try
            {
                IEnumerable<QA> entities = await _criteria
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

        public async Task<bool> SoftDeleteEntity(Guid id, int deleteStatusCode)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                var entityList = await _criteria.Add(Restrictions.Eq("Id", id))
                    .SetMaxResults(1)
                    .ListAsync<QA>();

                var entity = entityList.First(); // get first entity from list

                if (entity == null || entity.Status == deleteStatusCode) return false;
                entity.Status = deleteStatusCode;
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
    }
}
