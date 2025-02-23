using JoygameInventory.Business.Interface;
using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace JoygameInventory.Controllers
{
    [Authorize]

    public class ProductManagementController : Controller
    {
        private readonly IProductService _productService;
        private readonly IJoyStaffService _staffManager;
        private readonly IAssigmentService _assigmentService;
        private readonly EmailService _emailService;
        private readonly IMaintenanceService _maintenanceService;

        public ProductManagementController(IProductService productService, IJoyStaffService staffManager, IAssigmentService assigmentService, EmailService emailService, IMaintenanceService maintenanceService)
        {
            _productService = productService;
            _staffManager = staffManager;
            _assigmentService = assigmentService;
            _emailService = emailService;
            _maintenanceService = maintenanceService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductList(string category, string searchTerm)
        {
            var product = await _productService.GetProductAsync(searchTerm,category);
            ViewBag.SearchTerm = searchTerm;
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var model = await _productService.GetProductDetailsAsync(id);

            if (model == null)
            {
                TempData["ErrorMessage"] = "Ürünü detayına ulaşılamadı!";
                return RedirectToAction("ProductList");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            var model = await _productService.GetCreateViewAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDetails(ProductEditViewModel model)
        {
            try
            {
                var currentCategoryId = await _productService.GetCurrentCategoryIdAsync(model.Id);
                var product = await _productService.GetIdProductAsync(model.Id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Ürünü kaydederken bir hata oluştu!";
                    return RedirectToAction("ProductDetails", new { id = model.Id });
                }

                if (!await _productService.ProductBarkodUnique(model.ProductBarkod))
                {
                    TempData["ErrorMessage"] = $"{model.ProductBarkod} bu barkod başka ürün için kullanımda";
                    return RedirectToAction("ProductDetails", new { id = model.Id });
                }

                // Servisteki metodla ürün güncelleniyor
                await _productService.UpdateProductAsync(model);

                TempData["SuccessMessage"] = "Ürün Başarıyla Güncellendi";
                return RedirectToAction("ProductDetails", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("ProductDetails", new { id = model.Id });
            }
        }



        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductEditViewModel model)
        {
            if (!await _productService.ProductBarkodUnique(model.ProductBarkod))
            {
                TempData["ErrorMessage"] = "Girdiğiniz barkod numarası başka ürüne ait";
                return RedirectToAction("ProductCreate", model);
            }

            if (model.SelectedCategoryId <= 0)
            {
                TempData["ErrorMessage"] = "Lütfen Kategori Seçiniz";
                return RedirectToAction("ProductCreate", model);
            }

            var result = await _productService.CreateProduct(model);

            if (result)
            {
                TempData["SuccessMessage"] = "Ürün başarıyla oluşturuldu ve kategoriyle ilişkilendirildi!";
                return RedirectToAction("ProductDetails", new { id = model.Id });
            }
            else
            {
                TempData["ErrorMessage"] = "Ürün oluşturulamadı, lütfen girdiğiniz bilgileri kontrol ediniz";
                return RedirectToAction("ProductCreate", model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> ProductDelete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("ProductList");
        }

    }
}
