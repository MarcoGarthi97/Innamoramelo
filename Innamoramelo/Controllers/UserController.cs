using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Innamoramelo.Controllers
{
    public class UserController : AuthenticationController
    {
        public UserController(IConfiguration _config) : base(_config)
        {
            Config = _config;
        }

        public ActionResult<UserDTO> GetUser(string id)
        {
            try
            {
                AuthenticationAdmin();

                var userAPI = new UserAPI(Config);
                var userDTO = userAPI.GetUserById(id, TokenAdmin).Result;

                if(userDTO != null)
                    return Ok(userDTO.Name);
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }
    }
}
