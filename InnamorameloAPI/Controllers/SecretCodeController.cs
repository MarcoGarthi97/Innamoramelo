using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretCodeController : ControllerBase
    {
        static private AuthenticationAPI auth = new AuthenticationAPI();

        [HttpGet("GetSecretCode", Name = "GetSecretCode")]
        public SecretCodeDTO? GetSecretCode(bool reload = true)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var secretCodeAPI = new SecretCodeAPI();
                        var secretCodeDTO = secretCodeAPI.GetSecretCode(userDTO.Id, reload);

                        return secretCodeDTO;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        [HttpPost("ValidateUser", Name = "ValidateUser")]
        public bool ValidateUser(string code)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader) && !string.IsNullOrEmpty((string)code))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var secretCodeAPI = new SecretCodeAPI();
                        var secretCodeDTO = secretCodeAPI.GetSecretCode(userDTO.Id, false);
                        if (code == secretCodeDTO.Code && secretCodeDTO.Created.Value > DateTime.Now)
                        {
                            var result = secretCodeAPI.ValidateUser(userDTO.Id);

                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
