using AutoMapper.Internal;
using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        static private MyBadRequest badRequest = new MyBadRequest();

        [HttpPost("GetAuthentication", Name = "GetAuthentication")]
        public ActionResult<Token> GetAuthentication(AuthenticationDTO user)
        {
            try
            {
                if (Validator.ValidateFields(user))
                {
                    var accountAPI = new AccountAPI();
                    var account = accountAPI.GetAccount(user.Email, user.Password);

                    if(account != null)
                    {
                        var authAPI = new AuthenticationAPI();
                        var token = authAPI.GenerateToken(account);

                        return token;
                    }
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpGet("CheckAuthentication", Name = "CheckAuthentication")]
        public ActionResult<bool> CheckAuthentication()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    string headerValue = authHeader.ToString();
                    if (headerValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        string bearerToken = headerValue.Substring("Bearer ".Length).Trim();

                        AuthenticationAPI authentication = new AuthenticationAPI();
                        bool check = authentication.ValidateToken(bearerToken);

                        return check;
                    }
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpGet("CheckAuthenticationLevelAdmin", Name = "CheckAuthenticationLevelAdmin")]
        public ActionResult<bool> CheckAuthenticationLevelAdmin()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    string headerValue = authHeader.ToString();
                    if (headerValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        AuthenticationAPI authentication = new AuthenticationAPI();
                        bool check = authentication.CheckLevelUserByToken(authHeader);

                        return check;
                    }
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
