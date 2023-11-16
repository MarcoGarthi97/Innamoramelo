using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Innamoramelo.Controllers
{
    public class ProfileController : Controller
    {
        public JsonResult GetJobs(string filter)
        {
            //mettere il controllo login utente
            if (filter != null && filter.Trim() != "")
            {
                try
                {
                    Mongo mongo = new Mongo();
                    var jobs = mongo.GetJobs(filter).Result;

                    return Json(jobs);
                }
                catch (Exception ex)
                {

                }
            }

            return Json("");
        }

        public JsonResult GetMunicipality(string filter)
        {
            //mettere il controllo login utente
            if(filter != null && filter.Trim() != "")
            {
                try
                {
                    Mongo mongo = new Mongo();
                    var municipalities = mongo.GetMunicipality(filter).Result;

                    return Json(municipalities);
                }
                catch (Exception ex)
                {

                }
            }

            return Json("");
        }
    }
}
