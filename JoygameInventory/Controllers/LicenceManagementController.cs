using JoygameInventory.Business.Services;
using Microsoft.EntityFrameworkCore;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JoygameInventory.Business.Interface;

namespace JoygameInventory.Controllers
{
    [Authorize]
    public class LicenceManagementController : Controller
    {
        private readonly ILicenceService _licenceService;
        private readonly IJoyStaffService _staffManager;

        public LicenceManagementController(ILicenceService licenceService, IJoyStaffService staffManager)
        {
            _licenceService = licenceService;
            _staffManager = staffManager;
        }

        [HttpGet]
        public async Task<IActionResult> LicenceList(string searchTerm)
        {
            var licences = await _licenceService.GetLicenceAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(licences);
        }

        [HttpGet]
        public async Task<IActionResult> LicenceDetails(int id)
        {
            var model = await _licenceService.GetLicenceDetailsAsync(id);

            return View(model);
        }

        [HttpGet]
        public IActionResult LicenceCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LicenceCreate(LicenceEditViewModel model)
        {
            if (model.LicenceName == null || model.LicenceActiveDate == null || model.LicenceEndDate == null)
            {
                TempData["ErrorMessage"] = "Lütfen belirtilen alanları tam doldurunuz";
                return View(model);
            }
            if (!await _licenceService.IsLicenceUnique(model.LicenceName))
            {
                TempData["ErrorMessage"] = "Bu ünvana sahip başka takım var!";
                return View(model);
            }           
            var result = await _licenceService.AddLicence(model); 

            if (result)
            {
                TempData["SuccessMessage"] = "Lisans başarıyla oluşturuldu!";
                return RedirectToAction("LicenceList");
            }
            else
            {
                TempData["ErrorMessage"] = "Lisans Oluşturulurken Hata!";
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> NewAssigmentLicence(LicenceEditViewModel model)
        {
            // Lisans atama işlemini burada gerçekleştiriyorsunuz
            var result = await _licenceService.AssignLicenceToStaffAsync(model);

            if (result)
            {
                return Json(new { success = true, message = "Personel başarıyla atandı!" });
            }
            else
            {
                return Json(new { success = false, message = "Bir hata oluştu. Personel ataması yapılamadı." });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> LicenceDelete(int id)
        {
            // Lisans silme işlemi
            await _licenceService.DeleteLicenceAsync(id);

            // Başarı durumunda JSON cevabı döndür
            return Json(new { success = true, message = "Lisans başarıyla silindi!" });
        }
    }
}
