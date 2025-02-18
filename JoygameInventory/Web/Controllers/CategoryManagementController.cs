using JoygameInventory.Business.Interface;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JoygameInventory.Web.Controllers
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
            var categories = await _categoryService.SearchCategory(searchTerm);
            if (string.IsNullOrEmpty(searchTerm))
            {
                categories = await _categoryService.GetAllCategoriesAsync();
            }
            ViewBag.SearchTerm = searchTerm;
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDetails(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            var categoryProducts = await _categoryService.GetCategoryProductsAsync(category.Id);

            var model = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.Url,
                Products = categoryProducts
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryDetails(CategoryViewModel model)
        {
            var category = await _categoryService.GetCategoryByIdAsync(model.Id);
            if (category != null)
            {
                category.Name = model.Name;
                category.Url = model.Url;
                var result = await _categoryService.UpdateCategory(category);
                if (result)
                {
                    TempData["SuccessMessage"] = $"{model.Name} kategori başarıyla güncellendi";
                    return RedirectToAction("CategoryList");
                }
            }
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
            var categoryList = await _categoryService.GetAllCategoriesAsync();
            if (!await _categoryService.IsCateogryUrlUnique(model.Url))
            {
                TempData["ErrorMessage"] = $"{model.Name} kategori url'si mevcut, lütfen kontrol sağlayınız.";
                return RedirectToAction("CategoryList");
            }
            if (model.Name != null)
            {
                var category = new Category
                {
                    Name = model.Name,
                    Url = model.Url,
                };
                var result = await _categoryService.CreateCategory(category);
                if (result)
                {
                    TempData["SuccessMessage"] = $"{model.Name} kategori başarıyla oluşturuldu";
                    return RedirectToAction("CategoryList");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryDelete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category != null)
            {
                var result = await _categoryService.DeleteCategory(category.Id);
                if (result)
                {
                    TempData["SuccessMessage"] = $"{category.Name} kategori başarıyla silindi";
                    return RedirectToAction("CategoryList");
                }
            }
            return RedirectToAction("CategoryList");
        }
    }
}
