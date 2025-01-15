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
                var inventoryAssignments = await _assigmentservice.GetInventoryAssignmentsAsync(user.Id);

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

        public async Task<IActionResult> StaffList()
        {
            var staff = await _staffmanager.GetAllStaffsAsync();  // kullanıcıların listesini UserManager üzerinden çekiyoruz.
            return View("StaffManagement/StaffList", staff);
        }

        public async Task<IActionResult> StaffDetails(string id)
        {
            var staff = await _staffmanager.GetStaffByIdAsync(id);
            if (staff != null)
            {
                var inventoryAssignments = await _assigmentservice.GetInventoryAssignmentsAsync(staff.Id);

                var model = new StaffEditViewModel
                {
                    Id = staff.Id,
                    Name = staff.Name,
                    Surname = staff.Name,
                    Email = staff.Email,
                    PhoneNumber = staff.PhoneNumber,
                    Document = staff.Document,
                    InventoryAssigments = inventoryAssignments,

                };

                return View("UserManagement/StaffDetails", model);



            }
            return RedirectToAction("Index", "Home");

        }




    }
}
