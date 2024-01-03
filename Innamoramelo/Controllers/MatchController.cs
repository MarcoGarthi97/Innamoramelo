using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Innamoramelo.Controllers
{
    public class MatchController : AuthenticationController
    {
        public MatchController(IConfiguration _config) : base(_config)
        {
            Config = _config;
        }

        public ActionResult<List<string>?> GetMatches()
        {
            try
            {
                Authentication();

                var matchAPI = new MatchAPI(Config);
                var matchesDTO = matchAPI.GetMatches(Token).Result;

                var jsonUser = _privateController.GetSession("User");
                var userDTO = JsonConvert.DeserializeObject<UserDTO>(jsonUser);

                var ids = new List<string>();

                foreach(var matchDTO in matchesDTO)
                {
                    if (matchDTO.UsersId[0] != userDTO.Id)
                        ids.Add(matchDTO.UsersId[0]);
                    else
                        ids.Add(matchDTO.UsersId[1]);
                }

                if (matchesDTO != null)
                    return Ok(ids);
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }
    }
}
