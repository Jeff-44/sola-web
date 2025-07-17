using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sola_Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _service;
        public ServicesController(IServiceService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllServicesAsync());
        }

        [HttpGet(Name = "Create")]
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost(Name = "Create")]
        public async Task<IActionResult> AddService([Bind("Name,Description,IconUrl,IsActive,ServiceCategoryId")] Service model)
        {
            if (ModelState.IsValid) 
            {
                await _service.CreateServiceAsync(model);
                return RedirectToAction(nameof(Index), nameof(HomeController));
            }
            return View(model);
        }
    }
}
