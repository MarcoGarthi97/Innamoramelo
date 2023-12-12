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

        internal string? GetSession(string key)
        {
            if (httpContext.Session.Keys.Contains(key))
            {
                string json = httpContext.Session.GetString(key);

                return json;
            }
            else
                return null;
        }

        internal void Session(string key, string json)
        {
            httpContext.Session.SetString(key, json);
        }
    }
}
