using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Innamoramelo.Controllers
{
    public class HomeController : AuthenticationController
    {
        public HomeController(IConfiguration _config) : base(_config)
        {
            Config = _config;
        }

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
            if(Authentication())
                return View();
            else
                return View("Login");
        }

        public IActionResult Photo()
        {
            if (Authentication())
                return View();
            else
                return View("Login");
        }

        public IActionResult HomePage()
        {
            try
            {
                _privateController = new PrivateController(LoadContext());
                string json = _privateController.GetSession("User");

                if (json != "")
                {
                    var userDTO = JsonConvert.DeserializeObject<UserDTO>(json);

                    if (userDTO.CreateProfile == null || !userDTO.CreateProfile.Value)
                        return View("Profile");
                    else if (userDTO.CreateProfile.Value)
                        return View("HomePage");

                }
            }
            catch (Exception ex)
            {

            }

            return View("Login");
        }
    }
}