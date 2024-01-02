using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private static IConfiguration Config;
        static private AuthenticationAPI auth;
        static private MyBadRequest badRequest = new MyBadRequest();

        public MatchController(IConfiguration _config)
        {
            Config = _config;
            auth = new AuthenticationAPI(Config);
        }

        [HttpGet("GetMatch", Name = "GetMatch")]
        public ActionResult<MatchDTO> GetMatch(string receiverId)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var matchAPI = new MatchAPI(Config);

                        var matchDTO = new MatchDTO();
                        matchDTO.UsersId = new List<string>
                        {
                            userDTO.Id,
                            receiverId
                        };

                        matchDTO = matchAPI.GetMatchByUsersId(matchDTO);
                        
                        return Ok(matchDTO);
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpGet("GetAllMatches", Name = "GetAllMatches")]
        public ActionResult<List<MatchDTO>> GetAllMatches()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var matchAPI = new MatchAPI(Config);
                        var matchesDTO = matchAPI.GetAllMatches(userDTO.Id);

                        return Ok(matchesDTO);
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
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
