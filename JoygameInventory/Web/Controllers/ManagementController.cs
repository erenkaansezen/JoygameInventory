using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Web.Controllers
{
    //User Yönetimi olarak aratınca panel kullanıcılarının controller'ı
    //Envanter Yönetimi olarak aratınca Envanter Controller'ı
    //Staff Yönetimi olarak aratınca Şirket Personellerinin Controller'ı




    public class ManagementController : Controller
    {
        public InventoryContext _context;
        public UserManager<JoyUser> _usermanager;
        public JoyStaffService _staffmanager;
        public ProductService _productservice;
        public RoleManager<JoyRole> _rolemanager;
        public AssigmentService _assigmentservice;
        public ManagementController(UserManager<JoyUser> usermanager, ProductService productservice, RoleManager<JoyRole> rolemanager, AssigmentService assigmentservice, JoyStaffService staffmanager, InventoryContext context)
        {
            _usermanager = usermanager;
            _productservice = productservice;
            _rolemanager = rolemanager;
            _assigmentservice = assigmentservice;
            _staffmanager = staffmanager;
            _context = context;
        }


        //User Yönetimi
        [HttpGet]
        public async Task<IActionResult> UserList(string searchTerm)
        {
            var joypanelStaffs = await _staffmanager.SearchPanelStaff(searchTerm);

            // Eğer arama yapılmamışsa tüm staff'ı alıyoruz
            if (string.IsNullOrEmpty(searchTerm))
            {
                joypanelStaffs = _usermanager.Users.ToList();
            }

            // Arama terimi ViewBag içinde gönderiliyor
            ViewBag.SearchTerm = searchTerm;
            return View("UserManagement/UserList", joypanelStaffs);
        }
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            if (user != null)
            {
                ViewBag.Roles = await _rolemanager.Roles.Select(x => x.Name).ToListAsync();

                var model = new UserEditViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber

                };

                return View("UserManagement/UserDetails", model);



            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public async Task<IActionResult> UserDetails(UserEditViewModel model)
        {


            var user = await _usermanager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                // Şifreyi yalnızca boş değilse değiştirelim
                if (!string.IsNullOrEmpty(model.Password))
                {
                    // Şifre ve şifre onayı eşleşiyor mu kontrol et
                    if (model.Password != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("ConfirmPassword", "Parolalar eşleşmiyor.");
                        return View("UserManagement/UserDetails", model); // Eşleşmiyorsa hatayı döndürüyoruz
                    }

                    // Şifreyi değiştirme
                    await _usermanager.RemovePasswordAsync(user);
                    var addPasswordResult = await _usermanager.AddPasswordAsync(user, model.Password);

                    // Şifre eklerken bir hata olursa
                    if (!addPasswordResult.Succeeded)
                    {
                        foreach (var error in addPasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View("UserManagement/UserDetails", model);
                    }
                }

                var result = await _usermanager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi!";
                    return RedirectToAction("UserDetails", new { id = user.Id });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        if (!ModelState.IsValid)
                        {
                            return View("UserManagement/UserDetails", model); // Hatalı form gönderildiğinde formu tekrar göster
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UserRegister()
        {
            return View("UserManagement/UserRegister");

        }
        [HttpPost]
        public async Task<IActionResult> UserRegister(UserEditViewModel model)
        {


            var user = new JoyUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            // Kullanıcıyı oluşturuyoruz
            // Şifreyi yalnızca boş değilse ekleyelim
            if (!string.IsNullOrEmpty(model.Password))
            {
                var result = await _usermanager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu!";
                    return RedirectToAction("UserDetails", new { id = user.Id });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    // Eğer model geçerli değilse, hata mesajları görünür olacak şekilde formu tekrar göster.
                    return View("UserManagement/UserRegister", model);
                }
                // Şifre boşsa bir hata mesajı ekleyebiliriz
                ModelState.AddModelError("Password", "Parola gereklidir.");
            }

            return View("UserRegister", model);
        }

        [HttpPost]
        public async Task<IActionResult> UserDelete(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            if (user != null)
            {
                await _usermanager.DeleteAsync(user);
            }
            return RedirectToAction("UserList", "Management");
        }







        //Envanter Yönetimi
        public async Task<IActionResult> ProductList()
        {
            var product = await _productservice.GetAllProductsAsync();

            return View("ProductManagement/ProductList", product);
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var staff = await _productservice.GetIdProductAsync(id);
            if (staff != null)
            {
                var inventoryAssignments = await _assigmentservice.GetProductAssignmentsAsync(staff.Id);
                var previousAssignments = await _assigmentservice.GetPreviousAssignmentsAsync(staff.Id);
                var assignmentHistorys = await _assigmentservice.GetAssignmentHistoryAsync(id);
                var joystaff = await _staffmanager.GetAllStaffsAsync();
                var model = new ProductEditViewModel
                {
                    Id = staff.Id,
                    ProductName = staff.ProductName,
                    ProductBarkod = staff.ProductBarkod,
                    Description = staff.Description,
                    SerialNumber = staff.SerialNumber,
                    ProductAddDate = staff.ProductAddDate,
                    Status = staff.Status,
                    InventoryAssigments = inventoryAssignments,
                    JoyStaffs = joystaff,
                    AssigmentHistorys = assignmentHistorys



                };

                return View("ProductManagement/ProductDetails", model);



            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public async Task<IActionResult> ProductDetails(ProductEditViewModel model)
        {
            var products = await _productservice.GetIdProductAsync(model.Id);
            if (products != null)
            {
                // Ürün bilgilerini güncelliyoruz
                products.ProductName = model.ProductName;
                products.SerialNumber = model.SerialNumber;
                products.ProductBarkod = model.ProductBarkod;
                products.Description = model.Description;
                products.ProductAddDate = DateTime.Now;

                // Mevcut atama kaydını alıyoruz
                var currentAssignments = await _assigmentservice.GetProductAssignmentsAsync(products.Id);

                if (currentAssignments != null && currentAssignments.Any())
                {
                    var currentAssignment = currentAssignments.FirstOrDefault();

                    if (currentAssignment != null)
                    {
                        if (currentAssignment.PreviusAssigmenId.HasValue)
                        {
                            var previousUser = await _staffmanager.GetStaffByIdAsync(currentAssignment.PreviusAssigmenId.Value);
                            if (previousUser != null)
                            {
                                model.PreviousUserName = previousUser.Name;
                                model.PreviousUserSurname = previousUser.Surname;
                            }
                        }
                        else
                        {
                            // Eğer PreviusAssigmenId null ise yapılacak işlem
                            model.PreviousUserName = "Bilgi Yok";
                            model.PreviousUserSurname = "Bilgi Yok";
                        }


                        // Mevcut atama varsa ve kullanıcı değişmişse
                        if (currentAssignment.UserId != model.SelectedUserId)
                        {
                            // Önceki kullanıcıyı AssignmentHistory tablosuna kaydediyoruz
                            var assignmentHistory = new AssigmentHistory
                            {
                                ProductId = currentAssignment.ProductId,
                                UserId = currentAssignment.UserId,  // Eski kullanıcı ID'si
                                AssignmentDate = DateTime.Now
                            };
                            await _assigmentservice.AddAssignmentHistoryAsync(assignmentHistory);  // AssignmentHistory kaydını ekliyoruz

                            // Önceki kullanıcıyı PreviusAssigmenId'ye kaydediyoruz
                            currentAssignment.PreviusAssigmenId = currentAssignment.UserId;

                            // Yeni kullanıcıyı atıyoruz
                            currentAssignment.UserId = model.SelectedUserId.Value;

                            // Atama tarihini güncelliyoruz
                            currentAssignment.AssignmentDate = DateTime.Now;

                            // Atama kaydını güncelliyoruz
                            await _assigmentservice.UpdateAssigmentAsync(currentAssignment);
                        }
                        else
                        {
                            // Kullanıcı değişmemişse sadece ürünü güncelliyoruz
                            await _productservice.UpdateProductAsync(products);
                        }

                        return RedirectToAction("ProductDetails", new { id = model.Id });
                    }
                }
                else if (model.SelectedUserId.HasValue && model.SelectedUserId.Value != 0) // Atama kaydı yok ve kullanıcı seçildiyse
                {
                    // Yeni bir atama kaydı oluşturuyoruz
                    var newAssignment = new InventoryAssigment
                    {
                        ProductId = products.Id,
                        UserId = model.SelectedUserId.Value,  // Seçilen kullanıcı
                        AssignmentDate = DateTime.Now,
                        PreviusAssigmenId = null,  // null yapılıyor
                    };

                    // Yeni atamayı kaydediyoruz
                    await _assigmentservice.AddAssignmentAsync(newAssignment);

                    return RedirectToAction("ProductDetails", new { id = model.Id });
                }

                // Ürün bilgilerini güncelliyoruz
                await _productservice.UpdateProductAsync(products);

                return RedirectToAction("ProductDetails", new { id = model.Id });
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> AssigmentDelete(int userId, int inventoryAssigmentId)
        {
            // Kullanıcıyı veritabanından alıyoruz
            var staff = await _staffmanager.GetStaffByIdAsync(userId);
            var assigment = await _assigmentservice.GetAssignmentByIdAsync(inventoryAssigmentId);

            if (assigment != null)
            {

                // AssigmentHistory kaydını oluşturuyoruz
                var assignmentHistory = new AssigmentHistory
                {

                    ProductId = assigment.ProductId,
                    UserId = userId,
                    AssignmentDate = DateTime.Now,


                };
                await _assigmentservice.AddAssignmentHistoryAsync(assignmentHistory);  // AssignmentHistory kaydını ekliyoruz
            }

            await _assigmentservice.DeleteAssignmentAsync(inventoryAssigmentId);

            // Silme işlemi başarılı, kullanıcı detaylarına yönlendirelim
            return RedirectToAction("StaffDetails", new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(int id)
        {
            await _productservice.DeleteProductAsync(id);
            return RedirectToAction("ProductList", "Management");
        }








        //Staff Yönetimi
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
            return View("StaffManagement/StaffList", joyStaffs);
        }

        [HttpGet]   
        public async Task<IActionResult> StaffDetails(int id)
        {
            var staff = await _staffmanager.GetStaffByIdAsync(id);
            if (staff != null)
            {
                var inventoryAssignments = await _assigmentservice.GetUserAssignmentsAsync(staff.Id);

                var model = new StaffEditViewModel
                {
                    Id = staff.Id,
                    Name = staff.Name,
                    Surname = staff.Surname,
                    Email = staff.Email,
                    PhoneNumber = staff.PhoneNumber,
                    Document = staff.Document,
                    InventoryAssigments = inventoryAssignments,

                };

                return View("StaffManagement/StaffDetails", model);



            }
            return RedirectToAction("Index", "Home");

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

                bool updateSuccess = await _staffmanager.UpdateStaffAsync(staffs);

                if (updateSuccess)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi!";
                    return RedirectToAction("StaffList");
                }
                else
                {
                    ModelState.AddModelError("", "Güncelleme işlemi başarısız.");
                    return View("StaffManagement/StaffDetails", model);
                }
            }
            return View("StaffManagement/StaffCreate");

        }

        public async Task<IActionResult>StaffRegister()
        {
            return View("StaffManagement/StaffRegister");
        }
        [HttpPost]
        public async Task<IActionResult> StaffRegister(StaffEditViewModel model)
        {
            var staff = new JoyStaff
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            var result = await _staffmanager.CreateStaff(staff);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu!";
                return RedirectToAction("UserDetails", new { id = model.Id });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }




    }




}

