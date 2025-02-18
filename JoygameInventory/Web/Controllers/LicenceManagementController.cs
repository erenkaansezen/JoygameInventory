using JoygameInventory.Business.Services;
using Microsoft.EntityFrameworkCore;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JoygameInventory.Web.Controllers
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
            var licences = await _licenceService.SearchLicence(searchTerm);

            if (string.IsNullOrEmpty(searchTerm))
            {
                licences = await _licenceService.GetAllLicencesAsync();
            }

            ViewBag.SearchTerm = searchTerm;
            return View(licences);
        }

        [HttpGet]
        public async Task<IActionResult> LicenceDetails(int id)
        {
            var licence = await _licenceService.GetLicenceByIdAsync(id);
            var joyStaff = await _staffManager.GetAllStaffsAsync();

            if (licence != null)
            {
                var licenceUsers = await _licenceService.GetLicenceUserAssignmentsAsync(licence.Id);

                var model = new LicenceEditViewModel
                {
                    Id = licence.Id,
                    LicenceName = licence.LicenceName,
                    LicenceActiveDate = licence.LicenceActiveDate,
                    LicenceEndDate = licence.LicenceEndDate,
                    LicenceUser = licenceUsers,
                    JoyStaffs = joyStaff
                };

                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult LicenceCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LicenceCreate(LicenceEditViewModel model)
        {
            if (!await _licenceService.IsLicenceUnique(model.LicenceName))
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

                var result = await _licenceService.AddLicence(licence);
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
        public async Task<IActionResult> NewAssignmentLicence(LicenceEditViewModel model)
        {
            var licence = await _licenceService.GetLicenceByIdAsync(model.Id);
            var joyStaff = await _staffManager.GetAllStaffsAsync();

            if (licence != null)
            {
                model.JoyStaffs = joyStaff;

                if (model.SelectedStaffId.HasValue)
                {
                    var selectedStaff = joyStaff.FirstOrDefault(staff => staff.Id == model.SelectedStaffId.Value);

                    if (selectedStaff != null)
                    {
                        await _licenceService.AddAssignmentAsync(licence, selectedStaff);
                    }
                }
            }

            return RedirectToAction("LicenceDetails", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> LicenceAssignmentDelete(int licenceAssignmentId, int licenceId, LicenceEditViewModel model)
        {
            var assignment = await _licenceService.GetLicenceByIdAsync(licenceAssignmentId);
            var licence = await _licenceService.GetLicenceByIdAsync(licenceId);

            await _licenceService.DeleteLicenceAssignmentAsync(licenceAssignmentId);

            return RedirectToAction("LicenceDetails", new { id = licenceId });
        }

        [HttpPost]
        public async Task<IActionResult> LicenceDelete(int id)
        {
            await _licenceService.DeleteLicenceAsync(id);
            return RedirectToAction("LicenceDetails", "Management");
        }
    }
}
