using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;
using Sola_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sola_Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _service;
        private readonly IServiceCategoryService _categoryService;

        public ServicesController(IServiceService service, IServiceCategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllServicesAsync());
        }

        [HttpGet(Name = "Create")]
        public async Task<IActionResult> AddService()
        {
            var categories = await _categoryService.GetAllServiceCategoriesAsync();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");
            return View(new ServiceViewModel());
        }

        [HttpPost(Name = "Create")]
        public async Task<IActionResult> AddService(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateServiceAsync(model);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetAllServiceCategoriesAsync();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _service.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            var serviceVM = new ServiceViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                IconUrl = service.IconUrl,
                IsActive = service.IsActive,
                ServiceCategoryId = service.ServiceCategoryId
            };

            var categories = await _categoryService.GetAllServiceCategoriesAsync();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");
            return View(serviceVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateServiceAsync(model);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetAllServiceCategoriesAsync();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> DeleteService(int id)
        {
            await _service.DeleteServiceAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var service = await _service.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }
    }
}
