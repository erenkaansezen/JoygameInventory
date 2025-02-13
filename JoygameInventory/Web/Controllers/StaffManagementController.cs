using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    public class StaffManagementController : Controller
    {
        private readonly IJoyStaffService _staffmanager;
        private readonly IProductService _productservice;
        private readonly IAssigmentService _assigmentservice;
        private readonly ITeamService _teamservice;
        private readonly ILicenceService _licenceservice;
        private readonly EmailService _emailService;


        public StaffManagementController(IProductService productservice, IAssigmentService assigmentservice, IJoyStaffService staffmanager, ITeamService teamservice, ILicenceService licenceservice,EmailService emailService)
        {
            _productservice = productservice;
            _assigmentservice = assigmentservice;
            _staffmanager = staffmanager;
            _teamservice = teamservice;
            _licenceservice = licenceservice;
            _emailService = emailService;

        }


        [HttpGet]
        public async Task<IActionResult> StaffList(string searchTerm)
        {
            // Arama terimi varsa, arama sonuçlarını alıyoruz
            var joyStaffs = await _staffmanager.SearchStaff(searchTerm);


            // Eğer arama yapılmamışsa tüm staff'ı alıyoruz
            if (string.IsNullOrEmpty(searchTerm))
            {
                joyStaffs = await _staffmanager.GetAllStaffsAsync();
            }

            // Arama terimi ViewBag içinde gönderiliyor
            ViewBag.SearchTerm = searchTerm;

            // Arama sonuçlarını view'a gönderiyoruz
            return View(joyStaffs);
        }

        [HttpGet]
        public async Task<IActionResult> StaffDetails(int id)
        {
            var staff = await _staffmanager.GetStaffByIdAsync(id);
            if (staff != null)
            {
                var inventoryAssignments = await _assigmentservice.GetUserAssignmentsAsync(staff.Id);
                var userteams = await _teamservice.GetUserAssignmentsAsync(staff.Id);
                var userlicenses = await _licenceservice.GetUserLicenceAssignmentsAsync(staff.Id);
                var teams = await _teamservice.GetAllTeamsAsync();

                var model = new StaffEditViewModel
                {
                    Id = staff.Id,
                    Name = staff.Name,
                    Surname = staff.Surname,
                    Email = staff.Email,
                    PhoneNumber = staff.PhoneNumber,
                    Document = staff.Document,
                    InventoryAssigments = inventoryAssignments,
                    UserTeam = userteams,
                    Team = teams,
                    LicencesUser = userlicenses

                };

                return View(model);



            }
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> StaffRegister()
        {
            var teams = await _teamservice.GetAllTeamsAsync();
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


                // Kullanıcıdan gelen belge adını alıyoruz (belge adının tam yolu ve uzantısı ile birlikte)
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", documentName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(); // Dosya bulunamazsa 404 döner
                }

                // Dosyayı okuyup tarayıcıda görüntülenebilmesi için gönderiyoruz
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
            var staffs = await _staffmanager.GetStaffByIdAsync(model.Id);
            if (staffs != null)
            {

                staffs.Name = model.Name;
                staffs.Surname = model.Surname;
                staffs.Email = model.Email;
                staffs.PhoneNumber = model.PhoneNumber;

                var usersteam = await _teamservice.GetUserAssignmentsAsync(staffs.Id);


                if (usersteam != null && usersteam.Any())
                {
                    var userteam = usersteam.FirstOrDefault();
                    if (model.SelectedTeamId != null)
                    {
                        if (userteam.TeamId != model.SelectedTeamId)
                        {
                            userteam.TeamId = model.SelectedTeamId;


                        }

                        await _teamservice.UpdateTeamAsync(userteam);

                    }



                }

                bool updateSuccess = await _staffmanager.UpdateStaffAsync(staffs);



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
            if (!await _staffmanager.IsEmailUnique(model.Email))
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
                PhoneNumber = model.PhoneNumber,


            };
            var result = await _staffmanager.CreateStaff(staff);

            if (result)
            {
                var staffTeam = new UserTeam
                {
                    StaffId = staff.Id, // Ürün ID'si
                    TeamId = model.SelectedTeamId // Seçilen kategori ID'si
                };

                // ProductCategory kaydını ekliyoruz
                await _teamservice.AddUserTeam(staffTeam);
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
            _staffmanager.DeleteStaffAsync(id);
            return RedirectToAction("StaffList");
        }
        [HttpPost]
        public async Task<IActionResult> AddZimmetDocument(StaffEditViewModel model, IFormFile modelFile)
        {
            var staffs = await _staffmanager.GetStaffByIdAsync(model.Id); // id'yi almak için model üzerinden kullanabilirsiniz.
            if (staffs != null)
            {

                if (modelFile != null)
                {
                    // Dosya uzantısını al
                    var extension = Path.GetExtension(modelFile.FileName);

                    // Benzersiz dosya adı oluştur
                    var DocumentName = $"{model.Name}{model.Surname}{DateTime.Now:dd-MM-yyyy}{extension}";
                    // Dosyanın kaydedileceği yolu oluştur
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", DocumentName);

                    // Dosyayı belirtilen konuma kaydet
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await modelFile.CopyToAsync(stream);
                    }

                    // Slider modeline görsel adını ata
                    model.Document = DocumentName;


                }
                staffs.Document = model.Document;
                await _staffmanager.UpdateStaffAsync(staffs);
            }

            return RedirectToAction("StaffDetails", new { id = model.Id }); // Yönlendirme
        }

        [HttpPost]
        public async Task<IActionResult> DeleteZimmetDocument(StaffEditViewModel model)
        {
            var staffs = await _staffmanager.GetStaffByIdAsync(model.Id); // id'yi almak için model üzerinden kullanabilirsiniz.
            if (staffs != null)
            {
                staffs.Document = null;
                await _staffmanager.UpdateStaffAsync(staffs);
            }

            return RedirectToAction("StaffDetails", new { id = model.Id }); // Yönlendirme
        }

        [HttpPost]
        public async Task<IActionResult> LicenceUserAssigmentDelete(int LicenceAssigmentId, int userId)
        {
            var assigment = await _licenceservice.GetAssignmentByIdAsync(LicenceAssigmentId);
            var staff = await _staffmanager.GetStaffByIdAsync(userId);
            var licence = await _licenceservice.GetLicenceByIdAsync(assigment.LicenceId);
            if (staff != null)
            {
                var toEmailAddress = staff.Email;
                var subject = "Lisans Atamasının Kaldırılması Hakkında";
                var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                          $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                          $"<p style='text-align: center;'>" +  // Sadece bu satırda text-align: center; kullanarak resmin ortalanmasını sağlıyoruz
                          $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                          $"</p>" +  // Fotoğrafı bir <p> içine alıp, sadece onu ortalamış olduk.
                          $"<p>Merhaba <strong>{staff.Name}</strong>,</p>" +
                          $"<p>Aşağıda bilgileri belirtilen lisansın ataması sizden kaldırılmıştır</p>" +
                          $"<p><strong>Lisans :</strong> {licence.LicenceName}</p>" +
                          $"<p>Teşekkürler</p>" +
                          $"<p>İyi Çalışmalar</p>" +
                          $"</div></body></html>";
                var attachmentPaths = new List<string>();
                await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
            }
            await _licenceservice.DeleteLicenceAssigmentAsync(LicenceAssigmentId);

            return RedirectToAction("StaffDetails", new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> AssigmentDelete(int userId, int inventoryAssigmentId)
        {
            // Kullanıcıyı veritabanından alıyoruz
            var staff = await _staffmanager.GetStaffByIdAsync(userId);
            var assigment = await _assigmentservice.GetAssignmentByIdAsync(inventoryAssigmentId);
            var product = await _productservice.GetIdProductAsync(assigment.ProductId);

            if (assigment != null)
            {

                // AssigmentHistory kaydını oluşturuyoruz
                var assignmentHistory = new AssigmentHistory
                {

                    ProductId = assigment.ProductId,
                    UserId = userId,
                    AssignmentDate = DateTime.Now,


                };
    
                await _assigmentservice.AddAssignmentHistoryAsync(assignmentHistory);  
            }

            if (assigment.UserId != null)
            {
                var toEmailAddress = staff.Email;
                var subject = "Zimmetin Kaldırılması Hakkında";
                var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                          $"<div style='margin: 20px; background-color: white; padding: 20px;'>" +
                          $"<p style='text-align: center;'>" +  // Sadece bu satırda text-align: center; kullanarak resmin ortalanmasını sağlıyoruz
                          $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                          $"</p>" +  // Fotoğrafı bir <p> içine alıp, sadece onu ortalamış olduk.
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

            await _assigmentservice.DeleteAssignmentAsync(inventoryAssigmentId);
            return Json(new { success = true, message = "Veriler başarıyla gönderildi." });
        }
    }
}
