using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        static private AuthenticationAPI auth = new AuthenticationAPI();
        static private MyBadRequest badRequest = new MyBadRequest();

        [HttpGet("GetProfile", Name = "GetProfile")]
        public ActionResult<ProfileDTO> GetProfile()
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

        [HttpGet("GetProfileById", Name = "GetProfileById")]
        public ActionResult<ProfileDTO> GetProfileById(string id)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var profileAPI = new ProfileAPI();
                        var profile = profileAPI.GetProfileById(id);

                        if (profile != null) 
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

        [HttpGet("GetProfileByUserId", Name = "GetProfileByUserId")]
        public ActionResult<ProfileDTO> GetProfileByUserId(string id)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var profileAPI = new ProfileAPI();
                        var profile = profileAPI.GetProfileByUserId(id);

                        if (profile != null)
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

        [HttpPost("InsertProfile", Name = "InsertProfile")]
        public ActionResult<ProfileDTO> InsertProfile(ProfileViewModel profile)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var profileAPI = new ProfileAPI();
                        var profileinserted = profileAPI.GetProfileByUserId(userDTO.Id);
                        if(profileinserted == null)
                        {
                            var profileDTO = new ProfileDTO();
                            profileDTO.UserId = userDTO.Id;
                            Validator.CopyProperties(profile, profileDTO);

                            profileinserted = profileAPI.InsertProfile(profileDTO);

                            if (profileinserted != null)
                            {
                                userDTO.CreateProfile = true;

                                var userAPI = new UserAPI();
                                var userUpdate = userAPI.UpdateUser(userDTO);

                                return Ok(profileinserted);
                            }
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

        [HttpPatch("UpdateProfile", Name = "UpdateProfile")]
        public ActionResult<ProfileDTO> UpdateProfile(ProfileViewModel profileUpdate)
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
                        if (profile != null)
                        {
                            var profileDTO = new ProfileDTO();
                            profileDTO.UserId = userDTO.Id;
                            Validator.CopyProperties(profileUpdate, profileDTO);

                            var profileUpdated = profileAPI.UpdateProfile(profileDTO);

                            if (profileUpdated != null)
                                return Ok(profileUpdated);
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

        [HttpDelete("DeleteProfile", Name = "DeleteProfile")]
        public ActionResult<bool> DeleteProfile()
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
                        if (profile != null)
                        {
                            var result = profileAPI.DeleteProfileByUserId(userDTO.Id);

                            if (result)
                                return Ok(result);
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
