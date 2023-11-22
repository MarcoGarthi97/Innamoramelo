using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        static private AuthenticationAPI auth = new AuthenticationAPI();
        static private MyBadRequest badRequest = new MyBadRequest();

        [HttpGet("GetProfileByUser", Name = "GetProfileByUser")]
        public ActionResult<ProfileDTO> GetProfileByUser()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var profileAPI = new ProfileAPI();
                        var profile = profileAPI.GetProfileByUserId(userDTO.Id);

                        if(profile != null)
                            return Ok(profile);
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

        //TODO: Fare la chiamata GetProfileById con un altro tipo di autenticazione da livello admin

        [HttpPost("InsertProfile", Name = "InsertProfile")]
        public ActionResult<ProfileDTO> InsertProfile(ProfileDTO profile)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var profileAPI = new ProfileAPI();
                        var profileinserted = profileAPI.InsertProfile(profile);

                        if(profileinserted != null)
                            return Ok(profileinserted);
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

        [HttpPatch("UpdateProfile", Name = "UpdateProfile")]
        public ActionResult<ProfileDTO> UpdateProfile(ProfileDTO profile)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var profileAPI = new ProfileAPI();
                        var profileUpdated = profileAPI.UpdateProfile(profile);

                        if (profileUpdated != null)
                            return Ok(profileUpdated);
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

        [HttpDelete("DeleteProfile", Name = "DeleteProfile")]
        public ActionResult<bool> DeleteProfilebyUserId()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var profileAPI = new ProfileAPI();
                        var result = profileAPI.DeleteProfileByUserId(userDTO.Id);

                        if(result)
                            return Ok(result);
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

        //TODO: Fare la chiamata DeleteProfileById con un altro tipo di autenticazione da livello admin
    }
}
