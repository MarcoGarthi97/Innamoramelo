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
                var tokenJson = _privateController.GetSession("token");
                if (tokenJson != null)
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

                            Token = tokenDTO.Bearer;
                        }
                    }
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
                var authenticationAPI = new AuthenticationAPI(Config);
                var tokenJson = _privateController.GetSession("tokenAdmin");

                if (tokenJson == null)
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

        public ActionResult<bool> Logon(AuthenticationDTO authenticationDTO)
        {
            try
            {
                var authenticationAPI = new AuthenticationAPI(Config);
                var tokenDTO = authenticationAPI.GetBearerAsync(authenticationDTO).Result;

                if(tokenDTO != null)
                {
                    string json = JsonConvert.SerializeObject(tokenDTO);
                    _privateController.Session("token", json);

                    json = JsonConvert.SerializeObject(authenticationDTO);
                    _privateController.Session("credentials", json);

                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        public ActionResult<SecretCodeDTO> Register(UserCreateViewModel userCreateViewModel)
        {
            try
            {
                AuthenticationAdmin();

                var userAPI = new UserAPI(Config);
                var userDTO = userAPI.InsertUser(userCreateViewModel, TokenAdmin);

                if(userDTO != null)
                {

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
