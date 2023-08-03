using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Innamoramelo.Controllers
{
    public class PrivateController : Controller
    {
        internal User GetSessionUser()
        {
            var json = HttpContext.Session.GetString("InfoUser");
            if (json != null)
            {
                var user = JsonConvert.DeserializeObject<User>(json);
                if (user != null)
                {
                    return user;
                }
            }

            return new User();
        }
    }
}
