using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Controllers
{
    [Authorize]

    public class StaffManagementController : Controller
    {
        private readonly IJoyStaffService _staffManager;
        private readonly IProductService _productService;
        private readonly IAssigmentService _assigmentService;
        private readonly ITeamService _teamService;
        private readonly ILicenceService _licenceService;
        private readonly EmailService _emailService;


        public StaffManagementController(IProductService productService, IAssigmentService assigmentService, IJoyStaffService staffManager, ITeamService teamService, ILicenceService licenceService, EmailService emailService)
        {
            _productService = productService;
            _assigmentService = assigmentService;
            _staffManager = staffManager;
            _teamService = teamService;
            _licenceService = licenceService;
            _emailService = emailService;
        }


        [HttpGet]
        public async Task<IActionResult> StaffList(string searchTerm)
        {
            var joyStaffs = await _staffManager.SearchStaff(searchTerm);

            if (string.IsNullOrEmpty(searchTerm))
            {
                joyStaffs = await _staffManager.GetAllStaffsAsync();
            }

            ViewBag.SearchTerm = searchTerm;

            return View(joyStaffs);
        }

        [HttpGet]
        public async Task<IActionResult> StaffDetails(int id)
        {
            var staff = await _staffManager.GetStaffByIdAsync(id);
            if (staff != null)
            {
                var inventoryAssignments = await _assigmentService.GetUserAssignmentsAsync(staff.Id);
                var userTeams = await _teamService.GetUserAssignmentsAsync(staff.Id);
                var userLicences = await _licenceService.GetUserLicenceAssignmentsAsync(staff.Id);
                var teams = await _teamService.GetAllTeamsAsync();

                var model = new StaffEditViewModel
                {
                    Id = staff.Id,
                    Name = staff.Name,
                    Surname = staff.Surname,
                    Email = staff.Email,
                    PhoneNumber = staff.PhoneNumber,
                    Document = staff.Document,
                    InventoryAssigments = inventoryAssignments,
                    UserTeam = userTeams,
                    Team = teams,
                    LicencesUser = userLicences
                };

                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> StaffRegister()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            var model = new StaffEditViewModel
            {
                Team = teams
            };
            return View("StaffRegister", model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewZimmetDocument(string documentName)
        {
            if (documentName != null)
            {
                documentName = documentName?.Trim();

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", documentName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "application/pdf");
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> StaffDetails(StaffEditViewModel model)
        {
            var staffs = await _staffManager.GetStaffByIdAsync(model.Id);
            if (staffs != null)
            {
                staffs.Name = model.Name;
                staffs.Surname = model.Surname;
                staffs.Email = model.Email;
                staffs.PhoneNumber = model.PhoneNumber;

                var userTeams = await _teamService.GetUserAssignmentsAsync(staffs.Id);

                if (userTeams != null && userTeams.Any())
                {
                    var userTeam = userTeams.FirstOrDefault();
                    if (model.SelectedTeamId != null)
                    {
                        if (userTeam.TeamId != model.SelectedTeamId)
                        {
                            userTeam.TeamId = model.SelectedTeamId;
                        }

                        await _teamService.UpdateTeamAsync(userTeam);
                    }
                }

                bool updateSuccess = await _staffManager.UpdateStaffAsync(staffs);

                if (updateSuccess)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi!";
                    return RedirectToAction("StaffList");
                }
                else
                {
                    ModelState.AddModelError("", "Güncelleme işlemi başarısız.");
                    return View(model);
                }
            }
            return View("StaffManagement/StaffCreate");
        }

        [HttpPost]
        public async Task<IActionResult> StaffRegister(StaffEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("StaffManagement/StaffRegister", model);
            }
            if (!await _staffManager.IsEmailUnique(model.Email))
            {
                ModelState.AddModelError("Email", "Bu Kullanıcı Kayıtlı.");
                return View(model);
            }
            if (model.SelectedTeamId <= 0)
            {
                TempData["ErrorMessage"] = "Lütfen Kategori Seçiniz";
                return RedirectToAction("ProductCreate", model);
            }

            var staff = new JoyStaff
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            var result = await _staffManager.CreateStaff(staff);

            if (result)
            {
                var staffTeam = new UserTeam
                {
                    StaffId = staff.Id,
                    TeamId = model.SelectedTeamId
                };

                await _teamService.AddUserTeam(staffTeam);
                TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu!";
                return RedirectToAction("StaffDetails", new { id = staff.Id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulurken bir hata oluştu.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> StaffDelete(int id)
        {
            await _staffManager.DeleteStaffAsync(id);
            return RedirectToAction("StaffList");
        }

        [HttpPost]
        public async Task<IActionResult> AddZimmetDocument(StaffEditViewModel model, IFormFile modelFile)
        {
            var staffs = await _staffManager.GetStaffByIdAsync(model.Id);
            if (staffs != null)
            {
                if (modelFile != null)
                {
                    var extension = Path.GetExtension(modelFile.FileName);

                    var documentName = $"{model.Name}{model.Surname}{DateTime.Now:dd-MM-yyyy}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", documentName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await modelFile.CopyToAsync(stream);
                    }

                    model.Document = documentName;
                }
                staffs.Document = model.Document;
                await _staffManager.UpdateStaffAsync(staffs);
            }

            return RedirectToAction("StaffDetails", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteZimmetDocument(StaffEditViewModel model)
        {
            var staffs = await _staffManager.GetStaffByIdAsync(model.Id);
            if (staffs != null)
            {
                staffs.Document = null;
                await _staffManager.UpdateStaffAsync(staffs);
            }

            return RedirectToAction("StaffDetails", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> LicenceUserAssigmentDelete(int licenceAssigmentId, int userId)
        {
            var assigment = await _licenceService.GetAssignmentByIdAsync(licenceAssigmentId);
            var staff = await _staffManager.GetStaffByIdAsync(userId);
            var licence = await _licenceService.GetLicenceByIdAsync(assigment.LicenceId);
            if (staff != null)
            {
                var toEmailAddress = staff.Email;
                var subject = "Lisans Atamasının Kaldırılması Hakkında";
                var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                          $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                          $"<p style='text-align: center;'>" +
                          $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                          $"</p>" +
                          $"<p>Merhaba <strong>{staff.Name}</strong>,</p>" +
                          $"<p>Aşağıda bilgileri belirtilen lisansın ataması sizden kaldırılmıştır</p>" +
                          $"<p><strong>Lisans :</strong> {licence.LicenceName}</p>" +
                          $"<p>Teşekkürler</p>" +
                          $"<p>İyi Çalışmalar</p>" +
                          $"</div></body></html>";
                var attachmentPaths = new List<string>();
                await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
            }
            await _licenceService.DeleteLicenceAssigmentAsync(licenceAssigmentId);

            return RedirectToAction("StaffDetails", new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> AssigmentDelete(int userId, int inventoryAssigmentId)
        {
            var staff = await _staffManager.GetStaffByIdAsync(userId);
            var assigment = await _assigmentService.GetAssignmentByIdAsync(inventoryAssigmentId);
            var product = await _productService.GetIdProductAsync(assigment.ProductId);

            if (assigment != null)
            {
                var assignmentHistory = new AssigmentHistory
                {
                    ProductId = assigment.ProductId,
                    UserId = userId,
                    AssignmentDate = DateTime.Now,
                };

                await _assigmentService.AddAssignmentHistoryAsync(assignmentHistory);
            }

            if (assigment.UserId != null)
            {
                var toEmailAddress = staff.Email;
                var subject = "Zimmetin Kaldırılması Hakkında";
                var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                          $"<div style='margin: 20px; background-color: white; padding: 20px;'>" +
                          $"<p style='text-align: center;'>" +
                          $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                          $"</p>" +
                          $"<p>Merhaba <strong>{staff.Name}</strong>,</p>" +
                          $"<p>Aşağıda bilgileri belirtilen ürünün zimmeti sizden kaldırılmıştır</p>" +
                          $"<p><strong>Ürün :</strong> {product.ProductBrand} {product.ProductModel}</p>" +
                          $"<p><strong>Envanter Barkodu :</strong> {product.ProductBarkod}</p>" +
                          $"<p>Teşekkürler</p>" +
                          $"<p>İyi Çalışmalar</p>" +
                          $"</div></body></html>";
                var attachmentPaths = new List<string>();
                await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
            }

            await _assigmentService.DeleteAssignmentAsync(inventoryAssigmentId);
            return Json(new { success = true, message = "Veriler başarıyla gönderildi." });
        }
    }
}
