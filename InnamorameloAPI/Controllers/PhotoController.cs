﻿using InnamorameloAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace InnamorameloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhotoController : ControllerBase
    {
        private static IConfiguration Config;

        static private AuthenticationAPI auth;
        static private MyBadRequest badRequest = new MyBadRequest();

        public PhotoController(IConfiguration _config)
        {
            Config = _config;
            auth = new AuthenticationAPI(Config);
        }

        [HttpGet("GetPhotos", Name = "GetPhotos")]
        public ActionResult<List<PhotoDTO>> GetPhotos()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var photoAPI = new PhotoAPI(Config);
                        var photos = photoAPI.GetPhotosByUserId(userDTO.Id);

                        if(photos != null)
                            return Ok(photos);
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

        [HttpGet("GetPhotosByUserId", Name = "GetPhotosByUserId")]
        public ActionResult<List<PhotoDTO>> GetPhotosByUserId(string userId)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    if (auth.CheckLevelUserByToken(authHeader))
                    {
                        var photoAPI = new PhotoAPI(Config);
                        var photos = photoAPI.GetPhotosByUserId(userId);

                        if (photos != null)
                            return Ok(photos);
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

        [HttpPost("InsertPhoto", Name = "InsertPhoto")]
        public ActionResult<PhotoDTO> InsertPhoto(IFormFile file)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var photoAPI = new PhotoAPI(Config);
                        var photos = photoAPI.GetPhotosByUserId(userDTO.Id);

                        if (photos.Count < 3)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file.CopyTo(ms);

                                var photoInsert = new PhotoInsertModel(
                                    userDTO.Id,
                                    ms.ToArray(),
                                    Path.GetFileName(file.FileName),
                                    Path.GetExtension(file.FileName),
                                    photos.Count
                                    );

                                var insert = photoAPI.InsertPhoto(photoInsert);

                                return Ok(insert);
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

        [HttpPatch("UpdatePhoto", Name = "UpdatePhoto")]
        public ActionResult<PhotoDTO> UpdatePhoto(PhotoUpdateModel updateModel)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var photoModel = new PhotoInsertModel();
                        Validator.CopyProperties(updateModel, photoModel);

                        var photoAPI = new PhotoAPI(Config);
                        var update = photoAPI.UpdatePhoto(photoModel);

                        return Ok(update);
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

        [HttpDelete("DeletePhoto", Name = "DeletePhoto")]
        public ActionResult<bool> DeletePhoto(string id)
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var photoAPI = new PhotoAPI(Config);
                        var photoDTO = photoAPI.GetPhotoById(new ObjectId(id));

                        if(photoDTO.UserId == userDTO.Id)
                        {
                            var delete = photoAPI.DeletePhotoById(photoDTO);
                            return Ok(delete);
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

        [HttpDelete("DeletePhotosByUserId", Name = "DeletePhotosByUserId")]
        public ActionResult<bool> DeletePhotosByUserId()
        {
            try
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var userDTO = auth.GetUserByToken(authHeader);
                    if (userDTO != null)
                    {
                        var photoAPI = new PhotoAPI(Config);

                        var delete = photoAPI.DeletePhotosByIdUser(userDTO.Id);
                        return Ok(delete);
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
