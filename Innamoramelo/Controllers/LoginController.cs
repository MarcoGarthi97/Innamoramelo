using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Innamoramelo.Controllers
{
    public class LoginController : AuthenticationController
    {
        public LoginController(IConfiguration _config) : base(_config)
        {
            Config = _config;
        }

        private bool SetLogin(AuthenticationDTO authenticationDTO)
        {
            try
            {
                var authenticationAPI = new AuthenticationAPI(Config);
                var tokenDTO = authenticationAPI.GetBearerAsync(authenticationDTO).Result;

                if (tokenDTO.Bearer != null)
                {
                    var userAPI = new UserAPI(Config);
                    var userDTO = userAPI.GetUser(tokenDTO.Bearer).Result;

                    _privateController = new PrivateController(LoadContext());

                    string json = JsonConvert.SerializeObject(tokenDTO);
                    _privateController.Session("Token", json);

                    json = JsonConvert.SerializeObject(authenticationDTO);
                    _privateController.Session("Credentials", json);

                    json = JsonConvert.SerializeObject(userDTO);
                    _privateController.Session("User", json);

                    Token = tokenDTO.Bearer;

                    return true;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public ActionResult<bool> Logon(string json)
        {
            try
            {
                var authenticationDTO = JsonConvert.DeserializeObject<AuthenticationDTO>(json);
                var result = SetLogin(authenticationDTO);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        public ActionResult<bool> Register(string json)
        {
            try
            {
                var userModel = JsonConvert.DeserializeObject<UserCreateViewModel>(json);

                AuthenticationAdmin();

                var userAPI = new UserAPI(Config);
                var userDTO = userAPI.InsertUser(userModel, TokenAdmin).Result;

                if(userDTO.Id != null)
                {
                    var authenticationDTO = new AuthenticationDTO(userModel.Email, userModel.Password);

                    if (SetLogin(authenticationDTO))
                    {
                        Authentication();

                        var secretCodeAPI = new SecretCodeAPI(Config);
                        var sendMail = secretCodeAPI.GetSecretCode(Token).Result;

                        return Ok(sendMail);
                    }
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }
        
        public ActionResult<bool> ActivationUser(string json)
        {
            try
            {
                Authentication();

                var secretCodeAPI = new SecretCodeAPI(Config);
                var activate = secretCodeAPI.ValidateUser(json, Token).Result;

                return activate;
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }
    }
}
