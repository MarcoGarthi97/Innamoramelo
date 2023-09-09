using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Innamoramelo.Controllers
{
    public class PrivateController : Controller
    {
        private HttpContext httpContext;
        public PrivateController(HttpContext _httpContext) 
        { 
            httpContext = _httpContext;
        }

        internal User GetSessionUser()
        {
            string json = httpContext.Session.GetString("InfoUser");
            if (json != null)
            {
                var user = JsonConvert.DeserializeObject<User>(json);
                if (user != null)
                    return user;
            }

            return new User();
        }

        internal int? GetLogon()
        {
            int? logon = httpContext.Session.GetInt32("Logon");
            if (logon != null)
                return logon;

            return null;
        }

        internal void PutSessionUser(string json)
        {
            httpContext.Session.SetString("InfoUser", json);
        }

        internal void PutLogon(int val)
        {
            httpContext.Session.SetInt32("Logon", val);
        }
    }
}
