using JoygameInventory.Business.Interface;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JoygameInventory.Controllers
{
    [Authorize]
    public class CategoryManagementController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryManagementController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList(string searchTerm)
        {
            var categories = await _categoryService.GetCategoriesAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;

            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDetails(int id)
        {
            var model = await _categoryService.GetCategoryDetailsAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryDetails(CategoryViewModel model)
        {
            var result = await _categoryService.UpdateCategory(model);
            if (result)
            {
                TempData["SuccessMessage"] = $"{model.Name} kategori başarıyla güncellendi";
                return RedirectToAction("CategoryList");
            }
            TempData["ErrorMessage"] = "Kategori güncellenirken bir hata oluştu";
            return View(model);
        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CategoryCreate(CategoryViewModel model)
        {
            if (!await _categoryService.IsCateogryUrlUnique(model.Url))
            {
                TempData["ErrorMessage"] = $"{model.Name} kategori url'si mevcut, lütfen kontrol sağlayınız.";
                return RedirectToAction("CategoryList");
            }
            var result = await _categoryService.CreateCategory(model);
            if (result)
            {
                TempData["SuccessMessage"] = $"{model.Name} kategori başarıyla oluşturuldu";
                return RedirectToAction("CategoryList");
            }
            return View(model);
        }
        [HttpDelete]
        public async Task<IActionResult> CategoryDelete(int id)
        {
            var result = await _categoryService.DeleteCategory(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Silme işlemi sırasında bir hata oluştu veya kategori bulunamadı.";
            }

            return Json(new { success = result });
        }


    }
}
