using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("GetAuthentication", Name = "GetAuthentication")]
        public string GetAuthentication(User user)
        {
            Authentication authentication = new Authentication();
            string bearer = authentication.GenerateToken(user.Email, user.Password);

            return bearer;
        }

        [HttpGet("CheckAuthentication", Name = "CheckAuthentication")]
        public bool CheckAuthentication()
        {
            string bearerToken = "";

            // Ottieni l'header Authorization dalla richiesta HTTP
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                // Verifica se l'header inizia con "Bearer " e ottieni il token
                string headerValue = authHeader.ToString();
                if (headerValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    bearerToken = headerValue.Substring("Bearer ".Length).Trim();
                }
            }

            Authentication authentication = new Authentication();
            bool check = authentication.ValidateToken(bearerToken);

            return check;
        }
    }
}
