using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode.Impl;
using Sessions;

namespace TaskManager.Controllers
{
    public class DeveloperController : Controller
    {
        private readonly ILogger<DeveloperController> _logger;

        public DeveloperController(ILogger<DeveloperController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            IEnumerable<Developer> developers = session
                .Query<Developer>().OrderByDescending(d => d.CreatedAt)
                .Where(d => d.Status != 404)
                .ToList();

            transaction.Commit();
            return View(developers);
        }

        [HttpPost]
        public async Task<JsonResult> GetDevList(int draw, int start, int length, string filter_keywords, int filter_option = 0)
        {
            int totalRecord = 0;
            int filterRecord = 0;

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            IEnumerable<Developer> entities = await session
                .Query<Developer>().OrderByDescending(d => d.CreatedAt)
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
            IEnumerable<Developer> paginatdEntities = entities.Skip(start).Take(length)
                .OrderByDescending(d => d.CreatedAt).ToList().Where(d => d.Status != 404);

            List<object> entitiesList = new List<object>();
            foreach(var item in paginatdEntities)
            {
                string actionLink = $"<div class='w-75 btn-group' role='group'>" +
                    $"<a href='Developer/Edit/{item.Id}' class='btn btn-primary mx-2'><i class='bi bi-pencil-square'></i>Edit</a>" +
                    $"<button data-bs-target='#deleteDev' data-bs-toggle='ajax-modal' class='btn btn-danger mx-2 btn-delete'" +
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

            return Json(new { draw = draw, recordsTotal = totalRecord,
                recordsFiltered = filterRecord, data = entitiesList });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Developer entity)
        {
            if (ModelState.IsValid)
            {
                using var session = FluentNHibernateSession.Instance.OpenSession();
                using var transaction = session.BeginTransaction();
                await session.SaveAsync(entity);
                transaction.Commit();
                TempData["success"] = "Developer created successfully!";
                return RedirectToAction("Index");
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            var entity = await session.GetAsync<Developer>(id);
            transaction.Commit();
            if (entity == null || entity.Status == 404)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Developer entity)
        {
            if (ModelState.IsValid)
            {
                using var session = FluentNHibernateSession.Instance.OpenSession();
                using var transaction = session.BeginTransaction();
                await session.UpdateAsync(entity);
                transaction.Commit();
                TempData["success"] = "Developer updated successfully!";
                return RedirectToAction("Index");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            var entity = await session.GetAsync<Developer>(id);
            transaction.Commit();
            if (entity == null || entity.Status == 404)
            {
                return NotFound();
            }
            return PartialView("_DevDeletePartial", entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(Guid? id)
        {
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            var entity = await session.GetAsync<Developer>(id);
            if (entity == null || entity.Status == 404)
            {
                return NotFound();
            }
            entity.Status = 404;
            await session.UpdateAsync(entity);
            transaction.Commit();
            TempData["success"] = "Developer deleted successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            var entity = await session.GetAsync<Developer>(id);
            transaction.Commit();
            if (entity == null || entity.Status == 404) return NotFound();
            return View(entity);
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}