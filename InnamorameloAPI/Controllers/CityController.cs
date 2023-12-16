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

        [Obsolete("Method1 is deprecated, please use GetPlace instead.")]
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

        [HttpGet("GetPlace", Name = "GetPlace")]
        public ActionResult<List<CityDTO>> GetPlace(string filter)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        if (filter.Length > 2)
                        {
                            var geoDBAPI = new GeoDBAPI();
                            var geosDTO = geoDBAPI.GetPlace(filter);

                            return Ok(geosDTO);
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

        [HttpGet("GetPlaceDistance", Name = "GetPlaceDistance")]
        public ActionResult<int> GetPlaceDistance(int idPlace1, int idPlace2)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var geoDBAPI = new GeoDBAPI();
                        var distance = geoDBAPI.GetPlaceDistance(idPlace1, idPlace2);

                        return Ok(distance);
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
