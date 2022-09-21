using Entities;
using FluentNHibernate.Data;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using Services.QAService;

namespace TaskManager.Controllers
{
    public class QAController : Controller
    {
        private readonly ILogger<QAController> _logger;
        private readonly IQAService _service;
        public QAController(ILogger<QAController> logger, IQAService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ListQAs(int draw, int start, int length,
            string filter_keywords, int filter_option = 0)
        {
            var entities = await _service.ListEntities();

            int totalRecord = 0;
            int filterRecord = 0;

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
                    $"<button data-bs-target='#deleteQA' data-bs-toggle='ajax-modal' class='btn btn-danger mx-2 btn-qa-delete'" +
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
                if (!ModelState.IsValid) return View(entity);
                if (!await _service.CreateEntity(entity)) return View(entity);
                TempData["success"] = "QA Eng. created successfully!";
                return RedirectToAction("Index");
            }

            public async Task<IActionResult> Edit(Guid? id)
            {
                var entity = await _service.GetEntity(id);
                if (entity == null) return NotFound();
                return View(entity);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(QA entity)
            {
                if (!ModelState.IsValid) return View(entity);
                if (!await _service.UpdateEntity(entity)) return View(entity);
                TempData["success"] = "Developer updated successfully!";
                return RedirectToAction("Index");
            }

            public async Task<IActionResult> Delete(Guid? id)
            {
                string qADeletePartial = "_QADeletePartial";

                var entity = await _service.GetEntity(id);
                if (entity == null) return NotFound();

                return PartialView(qADeletePartial, entity);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeletePost(Guid? id)
            {
                if (!await _service.DeleteEntity(id)) return NotFound();
                TempData["success"] = "QA Eng. deleted successfully!";
                return RedirectToAction("Index");
            }

            public async Task<IActionResult> Details(Guid? id)
            {
                var entity = await _service.GetEntity(id);
                if (entity == null) return NotFound();
                return View(entity);
            }

        //     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //     public IActionResult Error()
        //     {
        //         return View("Error!");
        //     }
    }
}
