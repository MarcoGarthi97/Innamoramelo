using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace Innamoramelo.Controllers
{
    public class LoginController : Controller
    {
        private MyBadRequest badRequest = new MyBadRequest();
        private PrivateController _privateController;
        
        private string Token;
        private string TokenAdmin;

        private HttpContext LoadContext()
        {
            return HttpContext;
        }

        private IConfiguration Config;
        public LoginController(IConfiguration _config)
        {
            Config = _config;
        }

        private void Authentication()
        {            
            try
            {
                _privateController = new PrivateController(LoadContext());

                var tokenJson = _privateController.GetSession("token");
                if (tokenJson != "")
                {
                    var tokenDTO = JsonConvert.DeserializeObject<TokenDTO>(tokenJson);

                    var authenticationAPI = new AuthenticationAPI(Config);
                    var isExpired = authenticationAPI.ValidationBearerAsync(tokenDTO).Result;

                    if(isExpired == null || !isExpired.Value)
                    {
                        var credentialsJson = _privateController.GetSession("credentials");
                        if (credentialsJson != null)
                        {
                            var credentials = JsonConvert.DeserializeObject<AuthenticationDTO>(credentialsJson);
                            tokenDTO = authenticationAPI.GetBearerAsync(credentials).Result;

                            string json = JsonConvert.SerializeObject(tokenDTO);
                            _privateController.Session("token", json);                            
                        }
                    }

                    Token = tokenDTO.Bearer;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void AuthenticationAdmin()
        {
            try
            {
                _privateController = new PrivateController(LoadContext());

                var authenticationAPI = new AuthenticationAPI(Config);
                var tokenJson = _privateController.GetSession("tokenAdmin");

                if (tokenJson == "")
                {
                    var credentialsJson = System.IO.File.ReadAllText(Config["AdminCredentials"]);

                    var credentials = JsonConvert.DeserializeObject<AuthenticationDTO>(credentialsJson);
                    var tokenDTO = authenticationAPI.GetBearerAsync(credentials).Result;

                    string json = JsonConvert.SerializeObject(tokenDTO);
                    _privateController.Session("tokenAdmin", json);

                    TokenAdmin = tokenDTO.Bearer;
                }
                else
                {
                    var tokenDTO = JsonConvert.DeserializeObject<TokenDTO>(tokenJson);

                    var isExpired = authenticationAPI.ValidationBearerAsync(tokenDTO).Result;

                    if (isExpired == null || !isExpired.Value)
                    {
                        var credentialsJson = _privateController.GetSession("credentialsAdmin");
                        if (credentialsJson != null)
                        {
                            var credentials = JsonConvert.DeserializeObject<AuthenticationDTO>(credentialsJson);
                            tokenDTO = authenticationAPI.GetBearerAsync(credentials).Result;

                            string json = JsonConvert.SerializeObject(tokenDTO);
                            _privateController.Session("tokenAdmin", json);

                            TokenAdmin = tokenDTO.Bearer;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool SetLogin(AuthenticationDTO authenticationDTO)
        {
            try
            {
                var authenticationAPI = new AuthenticationAPI(Config);
                var tokenDTO = authenticationAPI.GetBearerAsync(authenticationDTO).Result;

                if (tokenDTO != null)
                {
                    _privateController = new PrivateController(LoadContext());

                    string json = JsonConvert.SerializeObject(tokenDTO);
                    _privateController.Session("token", json);

                    json = JsonConvert.SerializeObject(authenticationDTO);
                    _privateController.Session("credentials", json);

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

                return SetLogin(authenticationDTO);
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
