using AutoMapper.Internal;
using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretCodeController : ControllerBase
    {
        static private AuthenticationAPI auth = new AuthenticationAPI();
        static private MyBadRequest badRequest = new MyBadRequest();

        [HttpGet("GetSecretCode", Name = "GetSecretCode")]
        public ActionResult<bool> GetSecretCode(bool reload = true)
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

                        var googleAPI = new GoogleAPI();
                        var result = googleAPI.SendMail(userDTO.Email, secretCodeDTO.Code);

                        return result;
                    }
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
                }
            }
            catch (Exception ex)
            {
                return badRequest.CreateBadRequest("Internal Server Error", "An internal error occurred.", 500);
            }

            return badRequest.CreateBadRequest("Invalid request", "Invalid request", 400);
        }

        [HttpPost("ValidateUser", Name = "ValidateUser")]
        public ActionResult<bool> ValidateUser(string code)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
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
                    else
                        return badRequest.CreateBadRequest("Unauthorized", "User not authorizated", 404);
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
