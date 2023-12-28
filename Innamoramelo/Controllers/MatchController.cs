using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Innamoramelo.Controllers
{
    public class MatchController : AuthenticationController
    {
        public MatchController(IConfiguration _config) : base(_config)
        {
            Config = _config;
        }

        public ActionResult<List<MatchDTO>?> GetMatches()
        {
            try
            {
                Authentication();

                var matchAPI = new MatchAPI(Config);
                var matchesDTO = matchAPI.GetMatches(Token).Result;

                if (matchesDTO != null)
                    return Ok(matchesDTO);
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }
    }
}
