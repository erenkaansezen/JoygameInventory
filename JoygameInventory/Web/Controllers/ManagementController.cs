using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Web.Controllers
{
    public class ManagementController : Controller
    {
        public UserManager<JoyUser> _usermanager;
        public ProductService _productservice;
        public RoleManager<JoyRole> _rolemanager;
        public AssigmentService _assigmentservice;
        public ManagementController(UserManager<JoyUser> usermanager, ProductService productservice, RoleManager<JoyRole> rolemanager, AssigmentService assigmentservice)
        {
            _usermanager = usermanager;
            _productservice = productservice;
            _rolemanager = rolemanager;
            _assigmentservice = assigmentservice;
        }


        //User Yönetimi
        public async Task<IActionResult> UserList()
        {
            var user = _usermanager.Users.ToList(); // kullanıcıların listesini UserManager üzerinden çekiyoruz.
            return View("UserManagement/UserList", user);
        }
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);  // id bilgisini db'ye gönderip sonrasında id ile eşleşen user'ı çağırıyoruz

            if (user != null)
            {
                ViewBag.Roles = await _rolemanager.Roles.Select(x => x.Name).ToListAsync();
                var inventoryAssignments = await _assigmentservice.GetInventoryAssignmentsAsync(user.Id);

                var model = new UserEditViewModel
                {
                    Id = user.Id,
                    FullName = user.FirstName,
                    Email = user.Email,
                    SelectedRoles = await _usermanager.GetRolesAsync(user), // kullanıcının ilişkilendirilmiş olduğu rolleri getirir
                    InventoryAssigments = inventoryAssignments  // Envanterler
                };

                return View("UserManagement/UserDetails", model);



            }
            return RedirectToAction("Index", "Home");

        }

        //Envanter Yönetimi
        public async Task<IActionResult> ProductList()
        {
            var product =await _productservice.GetAllProductsAsync();
            return View("ProductManagement/ProductList", product);
        }
    }
}
