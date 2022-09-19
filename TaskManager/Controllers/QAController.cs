using Entities;
using FluentNHibernate.Data;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using Sessions;

namespace TaskManager.Controllers
{
    public class QAController : Controller
    {
        private readonly ILogger<QAController> _logger;
        public QAController(ILogger<QAController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            IEnumerable<QA> entities = await session
                .Query<QA>().OrderByDescending(d => d.CreatedAt)
                .Where(d => d.Status != 404)
                .ToListAsync();

            transaction.Commit();
            return View(entities);
        }

        [HttpPost]
        public async Task<JsonResult> GetQAList(int draw, int start, int length, string filter_keywords, int filter_option = 0)
        {
            int totalRecord = 0;
            int filterRecord = 0;

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            using var session = FluentNHibernateSession.Instance.OpenSession();
            using var transaction = session.BeginTransaction();
            IEnumerable<QA> entities = await session
                .Query<QA>().OrderByDescending(d => d.CreatedAt)
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
            IEnumerable<QA> paginatdEntities = entities.Skip(start).Take(length)
                .OrderByDescending(d => d.CreatedAt).ToList().Where(d => d.Status != 404);

            List<object> entitiesList = new List<object>();
            foreach (var item in paginatdEntities)
            {
                string actionLink = $"<div class='w-75 btn-group' role='group'>" +
                    $"<a href='QA/Edit/{item.Id}' class='btn btn-primary mx-2'><i class='bi bi-pencil-square'></i>Edit</a>" +
                    $"<button data-bs-target='#deleteQA' data-bs-toggle='ajax-modal' class='btn btn-danger mx-2 btn-delete'" +
                    $"data-qa-id='{item.Id}'><i class='bi bi-trash-fill'></i>Delete</button><a href='QA/Details/{item.Id}'" +
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

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = entitiesList
            });
        }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(QA entity)
            {
                if (ModelState.IsValid)
                {
                    using var session = FluentNHibernateSession.Instance.OpenSession();
                    using var transaction = session.BeginTransaction();
                    await session.SaveAsync(entity);
                    transaction.Commit();
                    TempData["success"] = "QA Eng. created successfully!";
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
                var entity = await session.GetAsync<QA>(id);
                transaction.Commit();
                if (entity == null || entity.Status == 404)
                {
                    return NotFound();
                }
                return View(entity);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(QA entity)
            {
                if (ModelState.IsValid)
                {
                    using var session = FluentNHibernateSession.Instance.OpenSession();
                    using var transaction = session.BeginTransaction();
                    await session.UpdateAsync(entity);
                    transaction.Commit();
                    TempData["success"] = "QA Eng. updated successfully!";
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
                var entity = await session.GetAsync<QA>(id);
                transaction.Commit();
                if (entity == null || entity.Status == 404)
                {
                    return NotFound();
                }
                return PartialView("_QADeletePartial", entity);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeletePost(Guid? id)
            {
                using var session = FluentNHibernateSession.Instance.OpenSession();
                using var transaction = session.BeginTransaction();
                var entity = await session.GetAsync<QA>(id);
                if (entity == null || entity.Status == 404)
                {
                    return NotFound();
                }
                entity.Status = 404;
                await session.SaveAsync(entity);
                transaction.Commit();
                TempData["success"] = "QA Eng. deleted successfully!";
                return RedirectToAction("Index");
            }

            public async Task<IActionResult> Details(Guid? id)
            {
                if (id == null) return NotFound();
                using var session = FluentNHibernateSession.Instance.OpenSession();
                using var transaction = session.BeginTransaction();
                var entity = await session.GetAsync<QA>(id);
                if (entity == null || entity.Status == 404) return NotFound();
                return View(entity);
            }

        //     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //     public IActionResult Error()
        //     {
        //         return View("Error!");
        //     }
    }
}
