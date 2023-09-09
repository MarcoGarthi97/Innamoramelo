﻿using Innamoramelo.Models;
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
                {
                    return user;
                }
            }

            return new User();
        }

        internal void PutSessionUser(string json)
        {
            httpContext.Session.SetString("InfoUser", json);
        }
    }
}