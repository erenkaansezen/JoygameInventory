using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    public class MaintenanceController : Controller
    {
        public MaintenanceService _maintenanceservice;
        public ProductService _productservice;

        public MaintenanceController(MaintenanceService maintenanceservice, ProductService productservice)
        {
            _maintenanceservice = maintenanceservice;
            _productservice = productservice;
        }
        public async Task<IActionResult> MaintenanceList(string searchTerm)
        {
            // Arama terimi varsa, arama sonuçlarını alıyoruz
            var Maintenances = await _maintenanceservice.SearchMaintenanceService(searchTerm);


            // Eğer arama yapılmamışsa tüm staff'ı alıyoruz
            if (string.IsNullOrEmpty(searchTerm))
            {
                Maintenances = await _maintenanceservice.GetAllServiceAsync();
            }

            // Arama terimi ViewBag içinde gönderiliyor
            ViewBag.SearchTerm = searchTerm;


            // Arama sonuçlarını view'a gönderiyoruz
            return View(Maintenances);
        }

        [HttpPost]
        public async Task<IActionResult> MaintenanceCreate(MaintenanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Hata mesajlarını loglayın veya görsel olarak ekrana yazdırın
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            if (ModelState.IsValid)
            {
                var maintenance = new Maintenance
                {
                   
                    ProductBarkod = model.SelectedProductBarkod,
                    CreatedAt = model.MaintenanceCreated,
                    MaintenanceDescription = model.MaintenanceDescription
                };
                var result = await _maintenanceservice.CreateMaintenance(maintenance);
                if (result)
                {
                    return RedirectToAction("MaintenanceList");
                }
            }
            return View(model);
        }
    }
}
