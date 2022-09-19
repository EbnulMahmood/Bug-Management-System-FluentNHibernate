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

        // client-side
        // public IActionResult LoadDevList() {
        //     IEnumerable<Developer> developers = _unitOfWork.Developers.GetAll()
        //         .OrderByDescending(d => d.CreatedAt)
        //         .Where(d => d.Status != 404);
        //     return Json(new { data = developers});
        // }

        [HttpPost]
        public JsonResult GetDevList(int draw, int start, int length, string filter_keywords, int filter_option = 0)
        {
            int totalRecord = 0;
            int filterRecord = 0;

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            IEnumerable<Developer> developers = session
                .Query<Developer>().OrderByDescending(d => d.CreatedAt)
                .Where(d => d.Status != 404)
                .ToList();

            transaction.Commit();

            //get total count of data in table
            totalRecord = developers.Count();

            if (!string.IsNullOrEmpty(filter_keywords))
            {
                developers = developers.Where(d => d.Name.ToLower().Contains(filter_keywords.ToLower()))
                .Where(d => d.Status != 404);
            }
            if (filter_option != 0)
            {
                developers = developers.Where(d => d.Status == filter_option)
                .Where(d => d.Status != 404);
            }

            // get total count of records after search 
            filterRecord = developers.Count();

            //pagination
            IEnumerable<Developer> devList = developers.Skip(start).Take(length)
                .OrderByDescending(d => d.CreatedAt).ToList().Where(d => d.Status != 404);

            List<object> dataList = new List<object>();
            foreach(var item in devList)
            {
                string actionLink = $"<div class='w-75 btn-group' role='group'>" +
                    $"<a href='Developer/Edit/{item.Id}'" +
                    $"class='btn btn-primary mx-2'><i class='bi bi-pencil-square'></i>Edit</a>" +
                    $"<button data-bs-target='#deleteDev' data-bs-toggle='ajax-modal' class='btn btn-danger mx-2 btn-delete'" +
                    $"data-dev-id='{item.Id}'>Delete</button><a href='Developer/Details/{item.Id}' class='btn btn-secondary mx-2'>" +
                    $"<i class='bi bi-trash-fill'></i>Details</a></div>";
                string statusConditionClass = item.Status == 1 ? "text-success" : "text-danger";
                string statusConditionText = item.Status == 1 ? "Active" : "Inactive";
                string status = $"<span class='{statusConditionClass}'>{statusConditionText}</span>";

                // Dictionary<string, string> dataItems = new Dictionary<string, string>();
                // dataItems.Add("name", item.Name);
                // dataItems.Add("status", status);
                // dataItems.Add("CustomStatus", item.Status == 1 ? "Active": "Inactive");
                // dataItems.Add("action", actionLink);

                List<string> dataItems = new List<string>();
                dataItems.Add(item.Name);
                dataItems.Add(status);
                dataItems.Add(actionLink);

                dataList.Add(dataItems);
            }

            var returnObj = new { draw = draw, recordsTotal = totalRecord,
                recordsFiltered = filterRecord, data = dataList };
            return Json(returnObj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Developer developer)
        {
            if (ModelState.IsValid)
            {
                using var session = FluentNHibernateSession.Instance.OpenSession();
                using var transaction = session.BeginTransaction();
                session.Save(developer);
                transaction.Commit();
                TempData["success"] = "Developer created successfully!";
                return RedirectToAction("Index");
            }
            return View(developer);
        }

        // public async Task<IActionResult> Edit(Guid? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //     var developer = await _session.Developers.FirstOrDefaultAsync(d => d.Id == id);
        //     if (developer == null || developer.Status == 404)
        //     {
        //         return NotFound();
        //     }
        //     return View(developer);
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(Developer developer)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         await _session.Save(developer);
        //         TempData["success"] = "Developer updated successfully!";
        //         return RedirectToAction("Index");
        //     }
        //     return View(developer);
        // }

        // public async Task<IActionResult> Delete(Guid? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //     var developer = await _session.Developers.FirstOrDefaultAsync(d => d.Id == id);
        //     if (developer == null || developer.Status == 404)
        //     {
        //         return NotFound();
        //     }
        //     return PartialView("_DevDeletePartial", developer);
        // }

        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeletePost(Guid? id)
        // {
        //     var developer = await _session.Developers.FirstOrDefaultAsync(d => d.Id == id);
        //     if (developer == null || developer.Status == 404)
        //     {
        //         return NotFound();
        //     }
        //     developer.Status = 404;
        //     await _session.Save(developer);
        //     TempData["success"] = "Developer deleted successfully!";
        //     return RedirectToAction("Index");
        // }

        // public async Task<IActionResult> Details(Guid? id)
        // {
        //     if (id == null) return NotFound();
        //     var developer = await _session.Developers.FirstOrDefaultAsync(d => d.Id == id);
        //     if (developer == null || developer.Status == 404) return NotFound();
        //     return View(developer);
        // }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}