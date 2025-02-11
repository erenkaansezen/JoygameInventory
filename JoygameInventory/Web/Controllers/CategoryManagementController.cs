using JoygameInventory.Business.Interface;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JoygameInventory.Web.Controllers
{
    public class CategoryManagementController : Controller
    {
        private readonly ICategoryService _categoryservice;
        public CategoryManagementController(ICategoryService categoryservice)
        {
            _categoryservice = categoryservice;
        }
        [HttpGet]
        public async Task<IActionResult> CategoryList(string searchTerm)
        {
            // Arama terimi varsa, arama sonuçlarını alıyoruz
            var categories = await _categoryservice.SearchCategory(searchTerm);
            // Eğer arama yapılmamışsa tüm staff'ı alıyoruz
            if (string.IsNullOrEmpty(searchTerm))
            {
                categories = await _categoryservice.GetAllCategoriesAsync();
            }
            // Arama terimi ViewBag içinde gönderiliyor
            ViewBag.SearchTerm = searchTerm;
            // Arama sonuçlarını view'a gönderiyoruz
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> CategoryDetails(int id)
        {
            var category = await _categoryservice.GetCategoryByIdAsync(id);
            var categoryproduct = await _categoryservice.GetCategoryProductsAsync(category.Id);


            var model = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.Url,
                Products = categoryproduct

            };

            return View(model);
            
        }
        [HttpPost]
        public async Task<IActionResult> CategoryDetails(CategoryViewModel model)
        {
            var category = await _categoryservice.GetCategoryByIdAsync(model.Id);
            if (category != null)
            {
                category.Name = model.Name;
                category.Url = model.Url;
                var result = await _categoryservice.UpdateCategory(category);
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
            var categoryList = await _categoryservice.GetAllCategoriesAsync();
            if (!await _categoryservice.IsCateogryUrlUnique(model.Url))
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
                var result = await _categoryservice.CreateCategory(category);
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
            var category = await _categoryservice.GetCategoryByIdAsync(id);
            if (category != null)
            {
                var result = await _categoryservice.DeleteCategory(category.Id);
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
