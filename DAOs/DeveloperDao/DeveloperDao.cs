using Entities;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs.DeveloperDao
{
    public class DeveloperDao : IDeveloperDao
    {
        private readonly ISession _session;
        private ITransaction? _transaction;

        public DeveloperDao(ISession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Developer>> ListDevelopersDescExclude404()
        {
            try
            {
                BeginTransaction();

                IEnumerable<Developer> entities = await _session
                    .Query<Developer>()
                    .OrderByDescending(d => d.CreatedAt)
                    .Where(d => d.Status != 404)
                    .ToListAsync();

                await Commit();
                return entities;
            }
            catch
            {
                await Rollback();
                throw new Exception();
            }
            finally
            {
                CloseTransaction();
            }
        }

        public async Task<bool> CreateDeveloper(Developer developerToCreate)
        {
            try
            {
                BeginTransaction();

                await _session.SaveAsync(developerToCreate);

                await Commit();
                return true;
            }
            catch
            {
                await Rollback();
                throw new Exception();
            }
            finally
            {
                CloseTransaction();
            }
        }

        public async Task<bool> UpdateDeveloper(Developer developerToUpdate)
        {
            try
            {
                BeginTransaction();

                await _session.UpdateAsync(developerToUpdate);

                await Commit();
                return true;
            }
            catch
            {
                await Rollback();
                throw new Exception();
            }
            finally
            {
                CloseTransaction();
            }
        }
        
        public async Task<bool> DeleteDeveloperInclude404(Guid? developerToGetId)
        {
            try
            {
                BeginTransaction();

                var entity = await _session.GetAsync<Developer>(developerToGetId);

                if (entity.Status == 404) return false;
                entity.Status = 404;
                await _session.UpdateAsync(entity);

                await Commit();
                return true;
            }
            catch
            {
                await Rollback();
                throw new Exception();
            }
            finally
            {
                CloseTransaction();
            }
        }

        public async Task<Developer?> GetDeveloperExclude404(Guid? developerToGetId)
        {
            try
            {
                BeginTransaction();

                var entity = await _session.GetAsync<Developer>(developerToGetId);

                await Commit();

                if (entity == null || entity.Status == 404) return null;
                return entity;
            }
            catch
            {
                await Rollback();
                throw new Exception();
            }
            finally
            {
                CloseTransaction();
            }
        }

        private void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        private async Task Commit()
        {
            await _transaction.CommitAsync();
        }

        private async Task Rollback()
        {
            await _transaction.RollbackAsync();
        }

        private void CloseTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}
