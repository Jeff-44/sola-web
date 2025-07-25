﻿using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Sola_Web.Controllers
{
    public class ServiceCategoryController : Controller
    {
        private readonly IServiceCategoryService _servicecategory;
        public ServiceCategoryController(IServiceCategoryService servicecategory)
        {
            _servicecategory = servicecategory;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _servicecategory.GetAllServiceCategoriesAsync());
        }
        [Authorize]
        public IActionResult AddServiceCategory()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddServiceCategory([Bind("Name,Description")] ServiceCategory model)
        {
            if (ModelState.IsValid)
            {
                await _servicecategory.AddServiceCategoryAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditServiceCategory(int id)
        {
            var obj = await _servicecategory.GetServiceCategoryByIdAsync(id);
            return View(obj);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditServiceCategory([Bind("Id,Name,Description")] ServiceCategory model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _servicecategory.UpdateServiceCategoryAsync(model);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
            
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> DeleteServiceCategory(int id)
        {
            var obj = await _servicecategory.GetServiceCategoryByIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }

            await _servicecategory.DeleteServiceCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
