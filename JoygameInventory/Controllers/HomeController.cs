using System.Diagnostics;
using JoygameInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
