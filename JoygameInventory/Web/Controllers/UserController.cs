using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JoygameInventory.Models.ViewModel;
using JoygameInventory.Business.Services;
using Microsoft.AspNetCore.Authorization;

namespace JoygameInventory.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<JoyUser> _usermanager;
        private readonly SignInManager<JoyUser> _signInManager;
        private readonly IJoyStaffService _staffmanager;


        public UserController(UserManager<JoyUser> usermanager, SignInManager<JoyUser> signInManager, IJoyStaffService staffmanager)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
            _staffmanager = staffmanager;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                TempData["ErrorMessage"] = "Lütfen Gerekli Alanları Doldurunuz";
                return RedirectToAction("Login");
            }
            var usermail = await _usermanager.FindByEmailAsync(model.Email);
            if (usermail != null)
            {
                var result = await _signInManager.PasswordSignInAsync(usermail, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserList");
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Hatalı Giriş, Bilgilerinizi Kontrol Ediniz!";
                return RedirectToAction("Login", model);
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }

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
            return View("UserList", joypanelStaffs);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            if (user != null)
            {

                var model = new UserEditViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber

                };

                return View(model);



            }
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public async Task<IActionResult> UserRegister()
        {
            return View("UserRegister");

        }



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
                        TempData["ErrorMessage"] = "Bu e-posta sistemde başka kullanıcıya kayıtlı!";
                        return RedirectToAction("UserDetails", model);
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
                        TempData["ErrorMessage"] = "Parolalar eşleşmiyor!";
                        return RedirectToAction("UserDetails", model);
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
                        return View(model);
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
                TempData["ErrorMessage"] = "Bu mail sistemde kayıtlı!";
                return RedirectToAction("UserRegister", model);
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
                        TempData["ErrorMessage"] = "Kullanıcı Kayıt Edilirken Hatayla karşılaşıldı";
                        return RedirectToAction("UserRegister", model);
                    
                }
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

            return RedirectToAction("UserList");
        }
    }
}
