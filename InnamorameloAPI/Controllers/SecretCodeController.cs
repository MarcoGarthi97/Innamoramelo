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
        public SecretCodeDTO GetSecretCode(bool reload = true)
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

            return null;
        }
    }
}
