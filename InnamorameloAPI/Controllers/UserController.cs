using AutoMapper;
using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        static private AuthenticationAPI auth = new AuthenticationAPI();

        [HttpGet("GetUser", Name = "GetUser")]
        public UserDTO GetUser()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var userDTO = auth.GetUserByToken(authHeader);
                if (userDTO != null)
                    return userDTO;
            }            

            return null;
        }

        [HttpPost("InsertUser", Name = "InsertUser")]
        public UserDTO InsertUser(UserCreateViewModel user)
        {
            if (Validator.ValidateFields(user))
            {
                var userAPI = new UserAPI();

                var loginCredential = new LoginCredentials(user.Email);
                if(!userAPI.CheckUser(loginCredential, true))
                {
                    var config = new MapperConfiguration(cfg => {
                        cfg.CreateMap<UserDTO, UserAPI>();
                    });

                    IMapper mapper = config.CreateMapper();

                    var userInsert = mapper.Map<UserAPI>(user);

                }
            }

            return null;
        }
    }
}
