using Iyzipay.Model.V2.Subscription;
using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace JoygameInventory.Web.Controllers
{
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
        public async Task<IActionResult> UserList()
        {
            var user = _usermanager.Users.ToList(); // kullanıcıların listesini UserManager üzerinden çekiyoruz.
            return View("UserManagement/UserList", user);
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
                    FullName = user.FirstName,
                    Email = user.Email,
                    SelectedRoles = await _usermanager.GetRolesAsync(user), 

                };

                return View("UserManagement/UserDetails", model);



            }
            return RedirectToAction("Index", "Home");

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
                    JoyStaffs = joystaff
                    
                    
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
                    var currentAssignment = currentAssignments?.FirstOrDefault();

                    if (currentAssignment != null)
                    {
                        // Eski kullanıcıyı alıyoruz (PreviusAssigmentUser ID'si ile)
                        var previousUser = await _staffmanager.GetStaffByIdAsync(currentAssignment.PreviusAssigmenId);

                        if (previousUser != null)
                        {
                            model.PreviousUserName = previousUser.Name;
                            model.PreviousUserSurname = previousUser.Surname;
                        }

                        // Eğer mevcut atama varsa ve kullanıcı değişmişse
                        if (currentAssignment.UserId != model.SelectedUserId)
                        {
                            // Eski kullanıcının ID'sini PreviusAssigmentUser'a kaydediyoruz
                            currentAssignment.PreviusAssigmenId = currentAssignment.UserId;

                            // Yeni kullanıcıyı atıyoruz
                            currentAssignment.UserId = model.SelectedUserId;

                            // Atama tarihini güncelliyoruz
                            currentAssignment.AssignmentDate = DateTime.Now;

                            // Eski atama kaydını güncelliyoruz
                            await _assigmentservice.UpdateProductAsync(currentAssignment);  // Var olan atama kaydını güncelliyoruz
                        }
                        else
                        {
                            await _productservice.UpdateProductAsync(products);

                            return RedirectToAction("ProductDetails", new { id = model.Id });
                        }
                    }
                }

                // Ürün bilgilerini güncelliyoruz
                await _productservice.UpdateProductAsync(products);

                return RedirectToAction("ProductDetails", new { id = model.Id });
            }

            return RedirectToAction("Index", "Home");
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




    }
}
