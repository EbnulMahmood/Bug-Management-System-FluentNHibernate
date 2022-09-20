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
using AutoMapper;
using Models;
using System.Web.Mvc;
using ModelStateDictionary = Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace Services.DeveloperService
{
    public class DeveloperService : IDeveloperService
    {
        private readonly ModelStateDictionary _modelState;
        private readonly IMapper _mapper;

        public DeveloperService(ModelStateDictionary modelState, IMapper mapper)
        {
            _modelState = modelState;
            _mapper = mapper;
        }

        public async Task<object> ListDevelopersDescExclude404(int draw, int start, int length,
            string filter_keywords, int filter_option = 0)
        {
            int totalRecord = 0;
            int filterRecord = 0;

            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();

            IEnumerable<DeveloperDto> entities = await session
                .Query<Developer>().Select(d =>
                new DeveloperDto()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Status = d.Status,
                    CreatedAt = d.CreatedAt,
                })
                .OrderByDescending(d => d.CreatedAt)
                .Where(d => d.Status != 404)
                .ToListAsync();

            transaction.Commit();

            //get total count of data in table
            totalRecord = entities.Count();

            if (!string.IsNullOrEmpty(filter_keywords))
            {
                entities = entities.Where(d => d.Name.ToLower().Contains(filter_keywords.ToLower()))
                .Where(d => d.Status != 404);
            }
            if (filter_option != 0)
            {
                entities = entities.Where(d => d.Status == filter_option)
                .Where(d => d.Status != 404);
            }

            // get total count of records after search 
            filterRecord = entities.Count();

            //pagination
            IEnumerable<DeveloperDto> paginatdEntities = entities.Select(d =>
                new DeveloperDto()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Status = d.Status,
                    CreatedAt = d.CreatedAt,
                }).Skip(start).Take(length)
                .OrderByDescending(d => d.CreatedAt).ToList().Where(d => d.Status != 404);

            List<object> entitiesList = new List<object>();
            foreach (var item in paginatdEntities)
            {
                string actionLink = $"<div class='w-75 btn-group' role='group'>" +
                    $"<a href='Developer/Edit/{item.Id}' class='btn btn-primary mx-2'><i class='bi bi-pencil-square'></i>Edit</a>" +
                    $"<button data-bs-target='#deleteDev' data-bs-toggle='ajax-modal' class='btn btn-danger mx-2 btn-dev-delete'" +
                    $"data-dev-id='{item.Id}'><i class='bi bi-trash-fill'></i>Delete</button><a href='Developer/Details/{item.Id}'" +
                    $"class='btn btn-secondary mx-2'><i class='bi bi-ticket-detailed-fill'></i>Details</a></div>";
                string statusConditionClass = item.Status == 1 ? "text-success" : "text-danger";
                string statusConditionText = item.Status == 1 ? "Active" : "Inactive";
                string status = $"<span class='{statusConditionClass}'>{statusConditionText}</span>";

                List<string> dataItems = new List<string>();
                dataItems.Add(item.Name);
                dataItems.Add(status);
                dataItems.Add(actionLink);

                entitiesList.Add(dataItems);
            }

            return new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = entitiesList
            };
        }
        
        public async Task<bool> CreateDeveloper(Developer developerToCreate)
        {
            try
            {
                if (!_modelState.IsValid) return false;

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
                if (!_modelState.IsValid) return false;

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

        public async Task<DeveloperDto?> GetDeveloperExclude404(Guid? developerToGetId)
        {
            try
            {
                if (developerToGetId == null) return null;

                using var session = FluentNHibernateSession.Instance.OpenSession();
                using var transaction = session.BeginTransaction();
                var entity = await session.GetAsync<Developer>(developerToGetId);
                transaction.Commit();

                var result = _mapper.Map<DeveloperDto>(entity);

                if (result == null || result.Status == 404) return null;

                return result;
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
