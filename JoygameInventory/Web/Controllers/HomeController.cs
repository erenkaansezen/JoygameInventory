using System.Diagnostics;
using JoygameInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
