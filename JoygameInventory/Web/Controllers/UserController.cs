using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JoygameInventory.Models.ViewModel;

namespace JoygameInventory.Web.Controllers
{
    public class UserController : Controller
    {
        public UserManager<JoyUser> _usermanager;
        private SignInManager<JoyUser> _signInManager;

        public UserController(UserManager<JoyUser> usermanager, SignInManager<JoyUser> signInManager)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var usermail = await _usermanager.FindByEmailAsync(model.Email);
            if (usermail != null)
            {
                var result = await _signInManager.PasswordSignInAsync(usermail, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserList", "Management");
                }

            }
            else
            {
                ModelState.AddModelError("", "Email Ve Şifre bilgini tam doldurunuz");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
    }
}
