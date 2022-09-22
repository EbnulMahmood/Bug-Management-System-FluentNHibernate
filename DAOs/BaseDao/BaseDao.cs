using Entities;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs.BaseDao
{
    public class BaseDao<T> : IBaseDao<T> where T : class
    {
        private readonly ISession _session;
        private readonly ICriteria _criteria;

        public BaseDao(ISession session)
        {
            _session = session;
            _criteria = _session.CreateCriteria<T>();
        }

        public async Task<IEnumerable<T>> ListEntities()
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                return await _criteria.ListAsync<T>();
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

        public async Task<T> LoadEntity(Guid id)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                var entity = await _criteria.Add(Restrictions.Eq("Id",id))
                    .SetMaxResults(1)
                    .ListAsync<T>();

                return entity.First();
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

        public async Task<bool> CreateEntity(T entity)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
                await _session.SaveAsync(entity);
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

        public async Task<bool> UpdateEntity(T entity)
        {
            using var transaction = _session.BeginTransaction();
            try
            {
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
