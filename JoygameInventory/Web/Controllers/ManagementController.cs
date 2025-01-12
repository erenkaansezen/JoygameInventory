using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    public class ManagementController : Controller
    {
        public UserManager<JoyUser> _usermanager;
        public ProductService _productservice;
        public ManagementController(UserManager<JoyUser> usermanager, ProductService productservice)
        {
            _usermanager = usermanager;
            _productservice = productservice;
        }


        //User Yönetimi
        public async Task<IActionResult> UserList()
        {
            var user = _usermanager.Users.ToList(); // kullanıcıların listesini UserManager üzerinden çekiyoruz.
            return View("UserManagement/UserList", user);
        }

        //Envanter Yönetimi
        public async Task<IActionResult> ProductList()
        {
            var product =await _productservice.GetAllProductsAsync();
            return View("ProductManagement/ProductList", product);
        }
    }
}
