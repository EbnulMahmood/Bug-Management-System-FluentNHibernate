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
            try
            {
                using var transaction = _session.BeginTransaction();
                IEnumerable<Developer> entities = await _developerCriteria
                    .AddOrder(Order.Desc("CreatedAt"))
                    .Add(!Restrictions.Eq("Status", 404))
                    .ListAsync<Developer>();
                transaction.Commit();

                return entities;
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<bool> CreateDeveloper(Developer developerToCreate)
        {
            try
            {
                using var transaction = _session.BeginTransaction();
                await _session.SaveAsync(developerToCreate);

                transaction.Commit();
                return true;
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<bool> UpdateDeveloper(Developer developerToUpdate)
        {
            try
            {
                using var transaction = _session.BeginTransaction();
                await _session.UpdateAsync(developerToUpdate);

                transaction.Commit();
                return true;
            }
            catch
            {
                throw new Exception();
            }
        }
        
        public async Task<bool> DeleteDeveloperInclude404(Guid? developerToGetId)
        {
            try
            {
                using var transaction = _session.BeginTransaction();
                var entity = await _session.GetAsync<Developer>(developerToGetId);

                if (entity.Status == 404) return false;
                entity.Status = 404;
                await _session.UpdateAsync(entity);

                transaction.Commit();
                return true;
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<Developer?> GetDeveloperExclude404(Guid? developerToGetId)
        {
            try
            {
                using var transaction = _session.BeginTransaction();
                var entity = await _session.GetAsync<Developer>(developerToGetId);
                transaction.Commit();

                if (entity == null || entity.Status == 404) return null;
                return entity;
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
