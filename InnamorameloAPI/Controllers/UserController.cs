using AutoMapper;
using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static IConfiguration Config;
        static private AuthenticationAPI auth;
        static private MyBadRequest badRequest = new MyBadRequest();

        public UserController(IConfiguration _config)
        {
            Config = _config;
            auth = new AuthenticationAPI(Config);
        }

        [HttpGet("GetUser", Name = "GetUser")]
        public ActionResult<UserDTO> GetUser()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                        return Ok(userDTO);
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

        [HttpGet("GetUserById", Name = "GetUserById")]
        public ActionResult<UserDTO> GetUserById(string id)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var userAPI = new UserAPI(Config);
                        var user = userAPI.GetUserById(id);

                        if (user != null)
                            return Ok(user);
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

        [HttpPost("InsertUser", Name = "InsertUser")]
        public ActionResult<UserDTO> InsertUser(UserCreateViewModel user)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var userAPI = new UserAPI(Config);

                        var loginCredential = new AuthenticationDTO(user.Email, user.Password);
                        if (!userAPI.CheckUser(loginCredential, true))
                        {
                            var account = new AccountDTO(loginCredential.Email, loginCredential.Password, "User");

                            var accountAPI = new AccountAPI(Config);
                            var insert = accountAPI.InsertAccount(account);

                            if (insert != null)
                            {
                                var userDTO = userAPI.InsertUser(user);

                                var secretCodeAPI = new SecretCodeAPI(Config);
                                var secretCodeDTO = secretCodeAPI.InsertSecretCode(userDTO.Id);

                                return Ok(userDTO);
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

        [HttpPatch("UpdateUser", Name = "UpdateUser")]
        public ActionResult<UserDTO> UpdateUser(UserUpdateViewModel user)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        Validator.CopyProperties(user, userDTO);

                        var userAPI = new UserAPI(Config);
                        var result = userAPI.UpdateUser(userDTO);

                        if (result != null)
                            return Ok(result);
                        else
                            return badRequest.CreateBadRequest("Update failed", "Update failed", 400);
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

        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        public ActionResult<bool> DeleteUser()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        //TODO: Elimanare tutto ciò che è dell'utente
                        var secretCodeAPI = new SecretCodeAPI(Config);
                        var result = secretCodeAPI.DeleteSecretCodeByUserId(userDTO.Id);

                        var accountAPI = new AccountAPI(Config);
                        result = accountAPI.DeleteAccount(userDTO.Email);

                        var userAPI = new UserAPI(Config);
                        result = userAPI.DeleteUser(userDTO.Id);

                        var profileAPI = new ProfileAPI(Config);
                        result = profileAPI.DeleteProfileByUserId(userDTO.Id);

                        var photoAPI = new PhotoAPI(Config);
                        result = profileAPI.DeleteProfileByUserId(userDTO.Id);

                        var likeAPI = new LikeAPI(Config);
                        result = likeAPI.DeleteLikesByUserId(userDTO.Id);

                        var chatAPI = new ChatAPI(Config);
                        result = chatAPI.DeleteChatByUserId(userDTO.Id);

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
    }
}
