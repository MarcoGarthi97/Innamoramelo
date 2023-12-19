using Innamoramelo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Innamoramelo.Controllers
{
    public class AuthenticationController : Controller
    {
        internal MyBadRequest badRequest = new MyBadRequest();
        internal PrivateController _privateController;

        public string Token;
        public string TokenAdmin;

        internal HttpContext LoadContext()
        {
            return HttpContext;
        }

        internal IConfiguration Config;
        public AuthenticationController(IConfiguration _config)
        {
            Config = _config;
        }

        internal bool Authentication()
        {
            try
            {
                var tokenDTO = ReloadAuthentication("Credentials", "Token");
                if (tokenDTO != null)
                {
                    Token = tokenDTO.Bearer;

                    return true;
                }

            }
            catch (Exception ex)
            {

            }

            return false;
        }

        internal void AuthenticationAdmin()
        {
            try
            {
                _privateController = new PrivateController(LoadContext());

                var authenticationAPI = new AuthenticationAPI(Config);
                var tokenJson = _privateController.GetSession("TokenAdmin");
                var tokenDTO = JsonConvert.DeserializeObject<TokenDTO>(tokenJson);

                if (tokenDTO == null || tokenDTO.Bearer == null)
                {
                    var credentialsJson = System.IO.File.ReadAllText(Config["AdminCredentials"]);

                    var credentials = JsonConvert.DeserializeObject<AuthenticationDTO>(credentialsJson);
                    tokenDTO = authenticationAPI.GetBearerAsync(credentials).Result;

                    _privateController.Session("AdminCredentials", credentialsJson);

                    string json = JsonConvert.SerializeObject(tokenDTO);
                    _privateController.Session("TokenAdmin", json);

                    TokenAdmin = tokenDTO.Bearer;
                }
                else
                {
                    tokenDTO = ReloadAuthentication("AdminCredentials", "TokenAdmin");

                    TokenAdmin = tokenDTO.Bearer;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private TokenDTO? ReloadAuthentication(string credentialsName, string tokenName)
        {
            try
            {
                _privateController = new PrivateController(LoadContext());

                var authenticationAPI = new AuthenticationAPI(Config);
                var tokenJson = _privateController.GetSession(tokenName);

                var tokenDTO = JsonConvert.DeserializeObject<TokenDTO>(tokenJson);

                if(tokenDTO != null)
                {
                    var isExpired = authenticationAPI.ValidationBearerAsync(tokenDTO).Result;

                    if (isExpired == null || !isExpired.Value)
                    {
                        var credentialsJson = _privateController.GetSession(credentialsName);
                        if (credentialsJson != null)
                        {
                            var credentials = JsonConvert.DeserializeObject<AuthenticationDTO>(credentialsJson);
                            tokenDTO = authenticationAPI.GetBearerAsync(credentials).Result;

                            string json = JsonConvert.SerializeObject(tokenDTO);
                            _privateController.Session(tokenName, json);

                        }
                    }
                }

                return tokenDTO;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
