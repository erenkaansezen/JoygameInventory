using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly IMaintenanceService _maintenanceservice;
        private readonly IProductService _productservice;

        public MaintenanceController(IMaintenanceService maintenanceservice, IProductService productservice)
        {
            _maintenanceservice = maintenanceservice;
            _productservice = productservice;
        }
        public async Task<IActionResult> MaintenanceList(string searchTerm)
        {
            var Maintenances = await _maintenanceservice.SearchMaintenanceService(searchTerm);


            if (string.IsNullOrEmpty(searchTerm))
            {
                Maintenances = await _maintenanceservice.GetAllServiceAsync();
            }

            ViewBag.SearchTerm = searchTerm;


            return View(Maintenances);
        }

        [HttpPost]
        public async Task<IActionResult> MaintenanceCreate(ProductEditViewModel model)
        {
            var MaintenanceList = await _maintenanceservice.GetAllServiceAsync();
            if (!await _maintenanceservice.IsProductBarkodUnique(model.ProductBarkod))
            {
                TempData["ErrorMessage"] = $"{model.ProductBarkod} ürüne ait devam eden servis var, lütfen kontrol sağlayınız.";
                return RedirectToAction("MaintenanceList");
            }
            if (model.ProductBarkod != null)
            {
                var maintenance = new Maintenance
                {
                    ProductBarkod = model.ProductBarkod,
                    ServiceAdress = model.ServiceAdress,
                    ServiceTitle = model.ServiceTitle,
                    CreatedAt = DateTime.Now,
                    MaintenanceDescription = model.MaintenanceDescription
                };
                var result = await _maintenanceservice.CreateMaintenance(maintenance);
                if (result)
                {
                    TempData["SuccessMessage"] = $"{model.ProductBarkod} ürünün başarıyla servis kaydı oluşturuldu";
                    return RedirectToAction("MaintenanceList");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MaintenanceDelete(int id)
        {

            await _maintenanceservice.DeleteMaintenanceAsync(id);
            return RedirectToAction("MaintenanceList");

        }

        [HttpPost]
        public async Task<IActionResult> MaintenanceHistory(int id)
        {
            var maintenance = await _maintenanceservice.GetMaintenanceByIdAsync(id);
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

                var result = await _maintenanceservice.MaintenanceHistoryAdd(history);

                if (result)
                {
                    TempData["SuccessMessage"] = $"{maintenance.ProductBarkod} Ürünün servis durumu tamamlanmıştır.";
                    await _maintenanceservice.DeleteMaintenanceAsync(id);
                    return RedirectToAction("MaintenanceList");
                }

                return View(maintenance);
            }
            return RedirectToAction("MaintenanceList");
        }

    }
}
