using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Innamoramelo.Controllers
{
    public class ProfileController : Controller
    {
        public JsonResult GetJobs(string filter)
        {
            //mettere il controllo login utente
            try
            {
                Mongo mongo = new Mongo();
                var jobs = mongo.GetJobs(filter).Result;

                return Json(jobs);
            }
            catch(Exception ex)
            {
                return Json("");
            }
        }
    }
}
