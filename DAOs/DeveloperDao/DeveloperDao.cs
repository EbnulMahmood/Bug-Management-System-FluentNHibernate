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
    }
}
