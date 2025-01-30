using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JoygameInventory.Web.Controllers
{
    //User Yönetimi olarak aratınca panel kullanıcılarının controller'ı
    //Envanter Yönetimi olarak aratınca Envanter Controller'ı
    //Staff Yönetimi olarak aratınca Şirket Personellerinin Controller'ı



    [Authorize]
    public class ManagementController : Controller
    {

        public InventoryContext _context;
        public UserManager<JoyUser> _usermanager;
        public JoyStaffService _staffmanager;
        public ProductService _productservice;
        public RoleManager<JoyRole> _rolemanager;
        public AssigmentService _assigmentservice;
        public ServerService _serverservice;
        public ManagementController(UserManager<JoyUser> usermanager, ProductService productservice, RoleManager<JoyRole> rolemanager, AssigmentService assigmentservice, JoyStaffService staffmanager, ServerService serverservice)
        {
            _usermanager = usermanager;
            _productservice = productservice;
            _rolemanager = rolemanager;
            _assigmentservice = assigmentservice;
            _staffmanager = staffmanager;
            _serverservice = serverservice;
        }


        //Kısa Arama Komutları

        //Get

        //List Get
        //Details Get
        //Create Get

        //Post

        //List Post
        //Details Post
        //Create Post
        //DeletePost


        //List Get
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
        [HttpGet]
        public async Task<IActionResult> ProductList(string category, string searchTerm)
        {            // İlk olarak tüm ürünleri alıyoruz
            var joyproducts = await _productservice.GetAllProductsAsync();

            // Eğer arama terimi varsa, arama sonuçlarına göre filtreleme yapıyoruz
            if (!string.IsNullOrEmpty(searchTerm))
            {
                joyproducts = await _productservice.SearchProduct(searchTerm); // Kategoriye göre filtreleme
            }

            // Eğer kategori seçilmişse, kategoriye göre filtreleme yapıyoruz
            if (!string.IsNullOrEmpty(category))
            {
                joyproducts = await _productservice.GetProductsByCategoryAsync(category); // Kategoriye göre filtreleme
            }


            return View("ProductManagement/ProductList", joyproducts);
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
            return View("StaffManagement/StaffList", joyStaffs);
        }

        [HttpGet]

        public async Task<IActionResult> ServerList(string searchTerm)
        {
            // Arama terimi varsa, arama sonuçlarını alıyoruz
            var servers = await _serverservice.SearchStaff(searchTerm);
            if (string.IsNullOrEmpty(searchTerm))
            {
                servers = await _serverservice.GetAllServersAsync();
            }


            ViewBag.SearchTerm = searchTerm;
            return View("ServerManagement/ServerList", servers);
        }

        //Details Get
        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> ServerDetails(int id)
        {
            // Sunucu bilgilerini alıyoruz
            var server = await _serverservice.GetServerByIdAsync(id);

            if (server != null)
            {


                // ViewModel'i oluşturuyoruz
                var model = new ServerEditViewModel
                {
                    Id = server.Id,
                    ServerName = server.ServerName,
                    IPAddress = server.IPAddress,
                    MACAddress = server.MACAddress,
                    OperatingSystem = server.OperatingSystem,
                    CPU = server.CPU,
                    RAM = server.RAM,
                    Storage = server.Storage,
                    Status = server.Status,
                    Location = server.Location,
                    DateInstalled = server.DateInstalled,
                    HostName = server.HostName,
                    SerialNumber = server.SerialNumber,
                    NetworkInterface = server.NetworkInterface,
                    PowerStatus = server.PowerStatus,
                    BackupStatus = server.BackupStatus,
                };

                return View("ServerManagement/ServerDetails", model);  // Sunucu detaylarını gösterecek view'a gönderiyoruz
            }

            // Sunucu bulunamadıysa anasayfaya yönlendiriyoruz
            return RedirectToAction("Index", "Home");
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
                var category = await _assigmentservice.GetAllStaffsAsync();

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
                    AssigmentHistorys = assignmentHistorys,
                    Categories = category,



                };

                return View("ProductManagement/ProductDetails", model);



            }
            return RedirectToAction("Index", "Home");

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

        //Create Get
        [HttpGet]
        public async Task<IActionResult> UserRegister()
        {
            return View("UserManagement/UserRegister");

        }

        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            var productcategory = await _productservice.GetAllProductCategoriesAsync();
            var category = await _assigmentservice.GetAllStaffsAsync();
            var model = new ProductEditViewModel
            {
                Categories = category
            };
            return View("ProductManagement/ProductCreate",model);

        }

        [HttpGet]
        public async Task<IActionResult> StaffRegister()
        {
            return View("StaffManagement/StaffRegister");
        }
        [HttpGet]
        public async Task<IActionResult> ServerCreate()
        {
            return View("ServerManagement/ServerCreate");
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



        //Details Post
        [HttpPost]
        public async Task<IActionResult> UserDetails(UserEditViewModel model)
        {


            var user = await _usermanager.FindByIdAsync(model.Id);
            if (user != null)
            {
                if (user.Email != model.Email)
                {
                    var existingUser = await _usermanager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        ModelState.Clear(); // Tüm hataları temizle

                        // E-posta zaten başka bir kullanıcıya ait
                        ModelState.AddModelError("Email", "Bu e-posta adresi başka bir kullanıcıya ait.");
                        return View("UserManagement/UserDetails", model);
                    }
                }
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;





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


                    }
                }


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

        [HttpPost]
        public async Task<IActionResult> ServerDetails(ServerEditViewModel model)
        {
            // Model doğrulamasını yapalım
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var server = await _serverservice.GetServerByIdAsync(model.Id);

            if (server != null)
            {
                // Nullable değerler için null kontrolü yapıyoruz
                if (model.RAM.HasValue)
                {
                    server.RAM = model.RAM.Value;
                }

                if (model.Storage.HasValue)
                {
                    server.Storage = model.Storage.Value;
                }

                server.ServerName = model.ServerName;
                server.IPAddress = model.IPAddress;
                server.MACAddress = model.MACAddress;
                server.OperatingSystem = model.OperatingSystem;
                server.CPU = model.CPU;
                server.Status = model.Status;
                server.Location = model.Location;
                server.DateInstalled = model.DateInstalled;
                server.HostName = model.HostName;
                server.SerialNumber = model.SerialNumber;
                server.NetworkInterface = model.NetworkInterface;
                server.PowerStatus = model.PowerStatus;
                server.BackupStatus = model.BackupStatus;

                // Sunucuyu güncelliyoruz
                await _serverservice.UpdateServerAsync(server);

                // Başarı mesajı ekliyoruz
                TempData["SuccessMessage"] = "Sunucu başarıyla güncellendi!";

                // Güncellenen sunucunun detaylarına yönlendiriyoruz
                return RedirectToAction("ServerDetails", new { id = server.Id });
            }
            else
            {
                // Sunucu bulunamazsa ana sayfaya yönlendirebiliriz
                return RedirectToAction("");
            }
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


                        // Mevcut atama varsa ve kullanıcı değişmişse
                        if (currentAssignment.UserId != model.SelectedUserId && model.SelectedUserId != null)
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
                            if (model.SelectedUserId != null)
                            {
                                currentAssignment.UserId = model.SelectedUserId.Value;

                            }

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


        //Create Post
        [HttpPost]
        public async Task<IActionResult> UserRegister(UserEditViewModel model)
        {


            var user = new JoyUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            if (!await _staffmanager.PanelIsEmailUnique(model.Email))
            {
                ModelState.AddModelError("Email", "Bu Kullanıcı Kayıtlı.");
                return View("UserManagement/UserRegister", model);
            }
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

            return View("UserManagement/UserRegister", model);
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
                return View("StaffManagement/StaffRegister", model);
            }

            var staff = new JoyStaff
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            var result = await _staffmanager.CreateStaff(staff);

            if (result)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu!";
                return RedirectToAction("StaffDetails", new { id = staff.Id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulurken bir hata oluştu.");
                return View("StaffManagement/StaffRegister", model);


            }

        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductEditViewModel model)
        {
            // Barkodun benzersiz olup olmadığını kontrol ediyoruz
            if (!await _productservice.ProductBarkodUnique(model.ProductBarkod))
            {
                TempData["ErrorMessage"] = "Girdiğiniz barkod numarası başka ürüne ait";
                return View("ProductCreate", model);
            }

            // Kategori seçilmediği durum için hata mesajı ekliyoruz
            if (model.SelectedCategoryId <= 0)
            {
                TempData["ErrorMessage"] = "Lütfen Kategori Seçiniz";
                return RedirectToAction("ProductCreate", model);
            }
            // Ürün nesnesini oluşturuyoruz
            var product = new Product
            {
                ProductName = model.ProductName,
                ProductBarkod = model.ProductBarkod,
                Description = model.Description,
                SerialNumber = model.SerialNumber,
                Categories = model.Categories
            };



            // Ürünü kaydetmeye başlıyoruz
            var result = await _productservice.CreateProduct(product);

            if (result) // Eğer ürün başarılı bir şekilde kaydedildiyse
            {
                // Ürün kaydedildikten sonra ProductCategory tablosuna ilişkilendirme ekliyoruz
                var productCategory = new ProductCategory
                {
                    ProductId = product.Id, // Ürün ID'si
                    CategoryId = model.SelectedCategoryId // Seçilen kategori ID'si
                };

                // ProductCategory kaydını ekliyoruz
                await _productservice.AddProductCategory(productCategory);

                TempData["SuccessMessage"] = "Ürün başarıyla oluşturuldu ve kategoriyle ilişkilendirildi!";
                return RedirectToAction("ProductDetails", new { id = product.Id });
            }
            else
            {
                TempData["ErrorMessage"] = "Ürün Oluşturulamadı, lütfen girdiğiniz bilgileri kontrol ediniz";
                return RedirectToAction("ProductCreate", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ServerCreate(ServerEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ServerManagement/ServerCreate", model);

            }

            var servers = new Servers
            {
                ServerName = model.ServerName,
                IPAddress = model.IPAddress,
                MACAddress = model.MACAddress,
                OperatingSystem = model.OperatingSystem,
                CPU = model.CPU,
                Status = model.Status,
                Location = model.Location,
                DateInstalled = model.DateInstalled,
                HostName = model.HostName,
                SerialNumber = model.SerialNumber,
                NetworkInterface = model.NetworkInterface,
                PowerStatus = model.PowerStatus,
                BackupStatus = model.BackupStatus,
                RAM = model.RAM.Value,
                Storage = model.Storage.Value

            };
            var result = await _serverservice.CreateServer(servers);

            if (result)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu!";
                return RedirectToAction("ServerList");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulurken bir hata oluştu.");
                return View("ServerManagement/ServerCreate", model);


            }

        }


        //Delete Post
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
        
        [HttpPost]
        public async Task<IActionResult> StaffDelete(int id)
        {
            _staffmanager.DeleteStaffAsync(id);
            return RedirectToAction("StaffList", "Management");
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
    }




}

