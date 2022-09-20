using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sessions;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using FluentNHibernate.Data;
using System.Web.Mvc;
using ModelStateDictionary = Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace Services.DeveloperService
{
    public class DeveloperService : IDeveloperService
    {
        public DeveloperService()
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
            try
            {
                using var session = FluentNHibernateSession.Instance.OpenSession();
                using var transaction = session.BeginTransaction();
                await session.SaveAsync(developerToCreate);
                transaction.Commit();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateDeveloper(Developer developerToUpdate)
        {
            try
            {
                using var session = FluentNHibernateSession.Instance.OpenSession();
                using var transaction = session.BeginTransaction();
                await session.UpdateAsync(developerToUpdate);
                transaction.Commit();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Developer?> GetDeveloperExclude404(Guid? developerToGetId)
        {
            try
            {
                if (developerToGetId == null) return null;

                using var session = FluentNHibernateSession.Instance.OpenSession();
                using var transaction = session.BeginTransaction();
                var entity = await session.GetAsync<Developer>(developerToGetId);
                transaction.Commit();

                if (entity == null || entity.Status == 404) return null;

                return entity;
            }
            catch
            {
                return null;
            }
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
    }
}
