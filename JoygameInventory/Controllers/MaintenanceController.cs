using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Controllers
{
    [Authorize]
    public class MaintenanceController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IProductService _productService;

        public MaintenanceController(IMaintenanceService maintenanceService, IProductService productService)
        {
            _maintenanceService = maintenanceService;
            _productService = productService;
        }

        public async Task<IActionResult> MaintenanceList(string searchTerm)
        {
            var maintenance = await _maintenanceService.GetServiceAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(maintenance);
        }

        [HttpPost]
        public async Task<IActionResult> MaintenanceCreate(ProductEditViewModel model)
        {
            if (!await _maintenanceService.IsProductBarkodUnique(model.ProductBarkod))
            {
                TempData["ErrorMessage"] = $"{model.ProductBarkod} ürüne ait devam eden servis var, lütfen kontrol sağlayınız.";
                return RedirectToAction("MaintenanceList");
            }
           var result = await _maintenanceService.CreateMaintenance(model);
           if (result)
           {
               TempData["SuccessMessage"] = $"{model.ProductBarkod} ürünün başarıyla servis kaydı oluşturuldu";
               return RedirectToAction("MaintenanceList");
           }
           return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MaintenanceDelete(int id)
        {
            await _maintenanceService.DeleteMaintenanceAsync(id);
            return RedirectToAction("MaintenanceList");
        }

        [HttpPost]
        public async Task<IActionResult> MaintenanceHistory(int id)
        {
            var maintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);
            if (maintenance != null)
            {
                var history = new MaintenanceHistory
                {
                    ProductBarkod = maintenance.ProductBarkod,
                    ServiceAdress = maintenance.ServiceAdress,
                    ServiceTitle = maintenance.ServiceTitle,
                    MaintenanceDescription = maintenance.MaintenanceDescription,
                    CreatedAt = maintenance.CreatedAt,
                    EndDate = DateTime.Now
                };

                var result = await _maintenanceService.MaintenanceHistoryAdd(history);

                if (result)
                {
                    TempData["SuccessMessage"] = $"{maintenance.ProductBarkod} Ürünün servis durumu tamamlanmıştır.";
                    await _maintenanceService.DeleteMaintenanceAsync(id);
                    return RedirectToAction("MaintenanceList");
                }

                return View(maintenance);
            }
            return RedirectToAction("MaintenanceList");
        }
    }
}
