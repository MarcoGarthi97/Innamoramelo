using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Innamoramelo.Controllers
{
    public class HomeController : Controller
    {
        private Authentication authentication = new();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View(); //Da cancellare
            return View(authentication.GetSite("Profile", HttpContext));
        }

        public IActionResult HomePage()
        {
            return View(authentication.GetSite("HomePage", HttpContext));
        }
    }
}