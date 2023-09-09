using Microsoft.AspNetCore.Mvc;

namespace Innamoramelo.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
