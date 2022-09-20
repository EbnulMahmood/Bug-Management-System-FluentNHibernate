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
using Services.DeveloperService;

namespace TaskManager.Controllers
{
    public class DeveloperController : Controller
    {
        private readonly ILogger<DeveloperController> _logger;
        private readonly IDeveloperService _service;

        public DeveloperController(ILogger<DeveloperController> logger)
        {
            _logger = logger;
            _service = new DeveloperService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ListDevelopers(int draw, int start, int length,
            string filter_keywords, int filter_option)
        {
            var entities = await _service.ListDevelopersDescExclude404(draw, start, length,
                filter_keywords, filter_option);

            return Json(entities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Developer entity)
        {
            if (!ModelState.IsValid) return View(entity);
            if (!await _service.CreateDeveloper(entity)) return View(entity);
            TempData["success"] = "Developer created successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var entity = await _service.GetDeveloperExclude404(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Developer entity)
        {
            if (!ModelState.IsValid) return View(entity);
            if (!await _service.UpdateDeveloper(entity)) return View(entity);
            TempData["success"] = "Developer updated successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            string devDeletePartial = "_DevDeletePartial";

            var entity = await _service.GetDeveloperExclude404(id);
            if (entity == null) return NotFound();

            return PartialView(devDeletePartial, entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(Guid? id)
        {
            if (!await _service.DeleteDeveloperInclude404(id)) return NotFound();
            TempData["success"] = "Developer deleted successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var entity = await _service.GetDeveloperExclude404(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}