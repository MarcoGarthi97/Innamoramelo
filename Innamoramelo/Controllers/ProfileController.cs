using Innamoramelo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Innamoramelo.Controllers
{
    public class ProfileController : AuthenticationController
    {
        public ProfileController(IConfiguration _config) : base(_config)
        {
            Config = _config;
        }

        public ActionResult<ProfileDTO?> InsertProfile(string json)
        {
            try
            {
                var profileModel = JsonConvert.DeserializeObject<ProfileViewModel>(json);

                Authentication();

                var profileAPI = new ProfileAPI(Config);
                var profileDTO = profileAPI.InsertProfile(profileModel, Token).Result;

                if(profileDTO != null)
                {
                    //string jsonProfileDTO = JsonConvert.SerializeObject(profileDTO);

                    //_privateController = new PrivateController(LoadContext());
                    //_privateController.Session("Profile", json);

                    return profileDTO;
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        public ActionResult<List<JobDTO>?> GetJob(string filter)
        {
            try
            {
                if(filter.Length > 2)
                {
                    AuthenticationAdmin();

                    var jobAPI = new JobAPI(Config);
                    var jobs = jobAPI.GetJob(filter, TokenAdmin).Result;

                    return Ok(jobs);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        public ActionResult<List<GeoDTO>?> GetCity(string filter)
        {
            try
            {
                if (filter.Length > 2)
                {
                    AuthenticationAdmin();

                    var cityAPI = new CityAPI(Config);
                    var cities = cityAPI.GetCity(filter, TokenAdmin).Result;

                    return Ok(cities);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }
    }
}
