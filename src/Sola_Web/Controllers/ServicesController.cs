using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;
using Sola_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Sola_Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _service;
        private readonly IServiceCategoryService _categoryService;
        private readonly IImageService _imageService;

        public ServicesController(IServiceService service, IServiceCategoryService categoryService, IImageService imageService)
        {
            _service = service;
            _categoryService = categoryService;
            _imageService = imageService;
        }
        public async Task<IActionResult> Index(int? categoryId)
        {
            var categories = await _categoryService.GetAllServiceCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);

            IEnumerable<Service> services = categoryId.HasValue
                ? await _service.GetServicesByCategoryAsync(categoryId.Value)
                : await _service.GetAllServicesAsync();

            ViewBag.SelectedCategoryId = categoryId;
            return View(services);
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
                if (model.IconImage != null)
                {
                    model.IconUrl = await _imageService.UploadAsync(model.IconImage, "services");
                }
                var service = new Service
                {
                    Name = model.Name,
                    Description = model.Description,
                    IconUrl = model.IconUrl,
                    IsActive = model.IsActive,
                    ServiceCategoryId = model.ServiceCategoryId
                };
                await _service.CreateServiceAsync(service);
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
                if (model.IconImage != null)
                {
                    model.IconUrl = await _imageService.UploadAsync(model.IconImage, "services");
                }
                var service = new Service
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    IconUrl = model.IconUrl,
                    IsActive = model.IsActive,
                    ServiceCategoryId = model.ServiceCategoryId
                };
                await _service.UpdateServiceAsync(service);
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
