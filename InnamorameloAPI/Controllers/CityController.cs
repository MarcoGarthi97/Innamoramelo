using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityController : Controller
    {
        static private AuthenticationAPI auth = new AuthenticationAPI();
        static private MyBadRequest badRequest = new MyBadRequest();

        [HttpGet("GetCity", Name = "GetCity")]
        public ActionResult<List<CityDTO>> GetCity(string filter)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        if (filter.Length > 2)
                        {
                            var cityAPI = new CityAPI();
                            var citiesDTO = cityAPI.GetCity(filter);

                            return Ok(citiesDTO);
                        }
                    }
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
