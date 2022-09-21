using Entities;
using NHibernate;
using NHibernate.Criterion;

namespace DAOs.DeveloperDao
{
    public class DeveloperDao : IDeveloperDao
    {
        private readonly ISession _session;
        private readonly ICriteria _developerCriteria;

        public DeveloperDao(ISession session)
        {
            _session = session;
            _developerCriteria = _session.CreateCriteria<Developer>();
        }

        public async Task<IEnumerable<Developer>> ListDevelopersDescExclude404()
        {
            using var transaction = _session.BeginTransaction();
            int deleteStatus = 404;
            try
            {
                IEnumerable<Developer> entities = await _developerCriteria
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

        public async Task<bool> CreateDeveloper(Developer developerToCreate)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                await _session.SaveAsync(developerToCreate);
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

        public async Task<bool> UpdateDeveloper(Developer developerToUpdate)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                await _session.UpdateAsync(developerToUpdate);
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
        
        public async Task<bool> DeleteDeveloperInclude404(Guid? developerToGetId)
        {
            using var transaction = _session.BeginTransaction();
            int deleteStatus = 404;
            try
            {
                var entity = await _session.GetAsync<Developer>(developerToGetId);

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

        public async Task<Developer?> GetDeveloperExclude404(Guid? developerToGetId)
        {
            using var transaction = _session.BeginTransaction();
            int deleteStatus = 404;
            try
            {
                var entity = await _session.GetAsync<Developer>(developerToGetId);

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
