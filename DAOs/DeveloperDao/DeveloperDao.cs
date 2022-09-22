using Entities;
using NHibernate;
using DAOs.BaseDao;
using NHibernate.Criterion;

namespace DAOs.DeveloperDao
{
    public class DeveloperDao : BaseDao<Developer>, IDeveloperDao
    {
        private readonly ISession _session;
        private readonly ICriteria _criteria;
        public DeveloperDao(ISession session) : base(session)
        {
            _session = session;
            _criteria = _session.CreateCriteria<Developer>();
        }

        public async Task<IEnumerable<Developer>> ListEntitiesOrderDescExcludeSoftDelete()
        {
            using var transaction = _session.BeginTransaction();
            int deleteStatus = 404;
            try
            {
                IEnumerable<Developer> entities = await _criteria
                    .AddOrder(Order.Desc("CreatedAt"))
                    .Add(!Restrictions.Eq("Status", deleteStatus))
                    .ListAsync<Developer>();
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
                    .ListAsync<Developer>();

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
