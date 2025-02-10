using JoygameInventory.Business.Services;
using Microsoft.EntityFrameworkCore;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;

using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    public class LicenceManagementController : Controller
    {
        private readonly ILicenceService _licenceservice;
        private readonly IJoyStaffService _staffmanager;

        public LicenceManagementController(ILicenceService licenceservice, IJoyStaffService staffmanager)
        {
            _licenceservice = licenceservice;
            _staffmanager = staffmanager;
        }

        [HttpGet]
        public async Task<IActionResult> LicenceList(string searchTerm)
        {
            // Arama terimi varsa, arama sonuçlarını alıyoruz
            var licences = await _licenceservice.SearchLicence(searchTerm);


            // Eğer arama yapılmamışsa tüm staff'ı alıyoruz
            if (string.IsNullOrEmpty(searchTerm))
            {
                licences = await _licenceservice.GetAllLicencesAsync();
            }

            // Arama terimi ViewBag içinde gönderiliyor
            ViewBag.SearchTerm = searchTerm;


            // Arama sonuçlarını view'a gönderiyoruz
            return View(licences);
        }
        [HttpGet]
        public async Task<IActionResult> LicenceDetails(int id)
        {
            var licence = await _licenceservice.GetLicenceByIdAsync(id);
            var joystaff = await _staffmanager.GetAllStaffsAsync();

            if (licence != null)
            {
                var licenceusers = await _licenceservice.GetLicenceUserAssignmentsAsync(licence.Id);

                var model = new LicenceEditViewModel
                {
                    Id = licence.Id,
                    LicenceName = licence.LicenceName,
                    LicenceActiveDate = licence.LicenceActiveDate,
                    LicenceEndDate = licence.LicenceEndDate,
                    LicenceUser = licenceusers,
                    JoyStaffs = joystaff
                };


                return View(model);



            }
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public async Task<IActionResult> LicenceCreate()
        {
            return View();
        }





        [HttpPost]
        public async Task<IActionResult> LicenceCreate(LicenceEditViewModel model)
        {

            if (!await _licenceservice.IsLicenceUnique(model.LicenceName))
            {
                TempData["ErrorMessage"] = "Bu ünvana sahip başka takım var!";
                return View(model);
            }
            if (model.LicenceActiveDate == null && model.LicenceEndDate == null)
            {
                TempData["ErrorMessage"] = "Lütfen belirtilen alanları tam doldurunuz";
                return View(model);
            }
            if (!string.IsNullOrEmpty(model.LicenceName))
            {

                var licence = new Licence
                {
                    LicenceName = model.LicenceName,
                    LicenceActiveDate = model.LicenceActiveDate,
                    LicenceEndDate = model.LicenceEndDate
                };
                var result = await _licenceservice.AddLicence(licence);
                if (result)
                {
                    TempData["SuccessMessage"] = "Lisans başarıyla oluşturuldu!";
                    return RedirectToAction("LicenceList");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Lisans oluşturulurken bir hata oluştu.");
                    return View(model);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Belirtilen alanı doldurunuz";
                return View(model);
            }

        }

                [HttpPost]
                public async Task<IActionResult> NewAssigmentLicence(LicenceEditViewModel model)
                {
                    var licence = await _licenceservice.GetLicenceByIdAsync(model.Id);
                    var joyStaff = await _staffmanager.GetAllStaffsAsync();

                    if (licence != null)
                    {
                        model.JoyStaffs = joyStaff;

                        if (model.SelectedStaffId.HasValue)
                        {
                            var selectedStaff = joyStaff.FirstOrDefault(staff => staff.Id == model.SelectedStaffId.Value);

                            if (selectedStaff != null)
                            {
                                await _licenceservice.AddAssignmentAsync(licence, selectedStaff);
                            }
                        }
                    }

                    return RedirectToAction("LicenceDetails", new { id = model.Id });
                }

        [HttpPost]
        public async Task<IActionResult> LicenceAssigmentDelete(int LicenceAssigmentId, int userId)
        {
            var staff = await _staffmanager.GetStaffByIdAsync(userId);
            var assigment = await _licenceservice.GetLicenceByIdAsync(LicenceAssigmentId);

            await _licenceservice.DeleteLicenceAssigmentAsync(LicenceAssigmentId);

            return RedirectToAction("LicenceDetails", new { id = userId });
        }
        
        [HttpPost]
        public async Task<IActionResult> LicenceDelete(int id)
        {
            await _licenceservice.DeleteLicenceAsync(id);
            return RedirectToAction("LicenceDetails", "Management");
        }
    }
}
