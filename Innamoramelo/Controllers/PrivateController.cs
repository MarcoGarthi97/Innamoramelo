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

        internal string GetSession(string key)
        {
            try
            {
                if (httpContext.Session.Keys.Contains(key))
                {
                    string json = httpContext.Session.GetString(key);

                    return json;
                }
            }
            catch(Exception ex)
            {

            }
            
            return "";
        }

        internal void Session(string key, string json)
        {
            httpContext.Session.SetString(key, json);
        }

        internal void RemoveSession(string key)
        {
            httpContext.Session.Remove(key);
        }
    }
}
