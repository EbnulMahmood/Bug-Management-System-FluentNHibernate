using Entities;
using NHibernate.Linq;
using Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs.DeveloperDao
{
    public class DeveloperDao : IDeveloperDao
    {
        public DeveloperDao()
        {
        }

        public async Task<IEnumerable<Developer>> ListDevelopersDescExclude404()
        {
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();

            IEnumerable<Developer> entities = await session
                .Query<Developer>()
                .OrderByDescending(d => d.CreatedAt)
                .Where(d => d.Status != 404)
                .ToListAsync();

            transaction.Commit();
            return entities;
        }

        public async Task<bool> CreateDeveloper(Developer developerToCreate)
        {
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            await session.SaveAsync(developerToCreate);
            transaction.Commit();
            return true;
        }

        public async Task<bool> UpdateDeveloper(Developer developerToUpdate)
        {
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            await session.UpdateAsync(developerToUpdate);
            transaction.Commit();
            return true;
        }
        
        public async Task<bool> DeleteDeveloperInclude404(Guid? developerToGetId)
        {
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            var entity = await session.GetAsync<Developer>(developerToGetId);

            if (entity.Status == 404) return false;
            entity.Status = 404;
            await session.UpdateAsync(entity);

            transaction.Commit();
            return true;
        }

        public async Task<Developer?> GetDeveloperExclude404(Guid? developerToGetId)
        {
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            var entity = await session.GetAsync<Developer>(developerToGetId);
            transaction.Commit();

            if (entity == null || entity.Status == 404) return null;

            return entity;
        }
    }
}
