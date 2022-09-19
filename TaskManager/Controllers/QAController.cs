// using Entities;
// using Microsoft.AspNetCore.Mvc;

// namespace TaskManager.Controllers
// {
//     public class QAController : Controller
//     {
//         private readonly ILogger<QAController> _logger;
//         private readonly IQAMapperSession _session;

//         public QAController(ILogger<QAController> logger,
//             IQAMapperSession session)
//         {
//             _session = session;
//             _logger = logger;
//         }

//         public async Task<IActionResult> Index()
//         {
//             IEnumerable<QA> qAs = await _session.QAs
//                 .Where(d => d.Status != 404)
//                 .OrderByDescending(q => q.CreatedAt)
//                 .ToListAsync();
//             return View(qAs);
//         }

//         public IActionResult Create()
//         {
//             return View();
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create(QA qA)
//         {
//             if (ModelState.IsValid)
//             {
//                 await _session.Save(qA);
//                 TempData["success"] = "QA Eng. created successfully!";
//                 return RedirectToAction("Index");
//             }
//             return View(qA);
//         }

//         public async Task<IActionResult> Edit(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//             var qA = await _session.QAs.FirstOrDefaultAsync(q => q.Id == id);
//             if (qA == null || qA.Status == 404)
//             {
//                 return NotFound();
//             }
//             return View(qA);
//         }

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(QA qA)
//         {
//             if (ModelState.IsValid)
//             {
//                 await _session.Save(qA);
//                 TempData["success"] = "QA Eng. updated successfully!";
//                 return RedirectToAction("Index");
//             }
//             return View(qA);
//         }

//         public async Task<IActionResult> Delete(Guid? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }
//             var qA = await _session.QAs.FirstOrDefaultAsync(q => q.Id == id);
//             if (qA == null || qA.Status == 404)
//             {
//                 return NotFound();
//             }
//             return PartialView("_QADeletePartial", qA);
//         }

//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeletePost(Guid? id)
//         {
//             var qA = await _session.QAs.FirstOrDefaultAsync(q => q.Id == id);
//             if (qA == null || qA.Status == 404)
//             {
//                 return NotFound();
//             }
//             qA.Status = 404;
//             await _session.Save(qA);
//             TempData["success"] = "QA Eng. deleted successfully!";
//             return RedirectToAction("Index");
//         }

//         public async Task<IActionResult> Details(Guid? id)
//         {
//             if (id == null) return NotFound();
//             var qA = await _session.QAs.FirstOrDefaultAsync(q => q.Id == id);
//             if (qA == null || qA.Status == 404) return NotFound();
//             return View(qA);
//         }

//         [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//         public IActionResult Error()
//         {
//             return View("Error!");
//         }
//     }
// }
